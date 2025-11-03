using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Comments.Models;
using SM.Application.Database;
using SM.Application.Shared.Models;
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
            .Where(c => c.Id == request.Id)
            .Select(c => new CommentQuery
            {
                ParentPostId = c.ParentPostId,
                Content = c.Content,
                TimeStamp = c.TimeStamp,
                CommentsCount = c.Replies != null ? c.Replies.Count() : 0,
                ReactionCounts = c.Reactions != null
                    ? c.Reactions
                        .GroupBy(r => r.Type)
                        .Select(rt => new ReactionCount(rt.Key, rt.Count()))
                    : new List<ReactionCount>()
            })
            .FirstOrDefaultAsync(ct);

        if (comment is null)
            return Result.Failure<CommentQuery>(new Error(UserErrors.NotFound), HttpStatusCode.NotFound);

        return Result.Success(comment);
    }
}
