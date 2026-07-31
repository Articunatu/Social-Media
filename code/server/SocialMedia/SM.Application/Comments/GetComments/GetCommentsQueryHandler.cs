using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Comments.Models;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Content;
using SM.Domain.Shared;

namespace SM.Application.Comments.GetComments;

internal class GetCommentsQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : IQueryHandler<GetCommentsQuery, PagedFeed<CommentQuery>>
{
    public async Task<Result<PagedFeed<CommentQuery>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var comments = await context.Comments
            .AsNoTracking()
            .Where(p => p.ParentPostId == request.ParentPostId && p.ParentCommentId == null && !p.IsDeleted)
            .Include(c => c.Reactions)
            .Include(c => c.Replies.Where(r => !r.IsDeleted))
            .ThenInclude(r => r.Reactions)
            .OrderByDescending(p => p.TimeStamp)
            .ToPagedFeed(request.Filter);

        var profiles = await context.GetProfileLookupAsync(comments.Values.SelectMany(CollectAuthorIds), cancellationToken);

        return Result.Success(new PagedFeed<CommentQuery>
        {
            Index = comments.Index,
            Order = comments.Order,
            SearchText = comments.SearchText,
            Values = comments.Values.Select(c => MapComment(c, profiles))
        });
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
