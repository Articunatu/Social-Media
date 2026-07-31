using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Comments.Models;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Content;
using SM.Domain.Shared;
using SM.Domain.Users;
using System.Net;

namespace SM.Application.Comments.GetCommentById;

internal class GetCommentByIdQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : IQueryHandler<GetCommentByIdQuery, CommentQuery>
{
    public async Task<Result<CommentQuery>> Handle(GetCommentByIdQuery request, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var comment = await context.Comments
            .AsNoTracking()
            .Include(c => c.Reactions)
            .Include(c => c.Replies.Where(r => !r.IsDeleted))
            .ThenInclude(r => r.Reactions)
            .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, ct);

        if (comment is null)
            return Result.Failure<CommentQuery>(new Error(UserErrors.NotFound), HttpStatusCode.NotFound);

        var profiles = await context.GetProfileLookupAsync(CollectAuthorIds(comment), ct);

        return Result.Success(MapComment(comment, profiles));
    }

    private static IEnumerable<Guid> CollectAuthorIds(Comment comment)
    {
        yield return comment.AuthorId;

        foreach (var reply in comment.Replies.Where(r => !r.IsDeleted))
            foreach (var id in CollectAuthorIds(reply))
                yield return id;
    }

    private static CommentQuery MapComment(Comment comment, IReadOnlyDictionary<Guid, ProfileInfo> profiles)
    {
        return new CommentQuery
        {
            PostId = comment.Id,
            AuthorId = comment.AuthorId,
            Author = profiles.TryGetValue(comment.AuthorId, out var profile)
                ? profile
                : new ProfileInfo(comment.AuthorId, string.Empty, string.Empty, null),
            Content = comment.Content,
            TimeStamp = comment.TimeStamp,
            ParentPostId = comment.ParentPostId,
            CommentsCount = comment.Replies.Count(r => !r.IsDeleted),
            ReactionCounts = comment.Reactions
                .GroupBy(r => r.Type)
                .Select(rt => new ReactionCount(rt.Key, rt.Count())),
            Replies = comment.Replies
                .Where(r => !r.IsDeleted)
                .OrderBy(r => r.TimeStamp)
                .Select(r => MapComment(r, profiles))
        };
    }
}
