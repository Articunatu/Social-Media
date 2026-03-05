using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Comments.Models;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Comments.GetComments;

internal class GetCommentsQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : IQueryHandler<GetCommentsQuery, PagedFeed<CommentQuery>>
{
    public async Task<Result<PagedFeed<CommentQuery>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var commentsQuery = context.Comments
            .AsNoTracking()
            .Where(p => p.ParentPostId == request.ParentPostId && p.ParentCommentId == null && !p.IsDeleted)
            .OrderByDescending(p => p.TimeStamp)
            .Select(p => new CommentQuery
            {
                PostId = p.Id,
                Content = p.Content,
                TimeStamp = p.TimeStamp,
                ParentPostId = request.ParentPostId,
                CommentsCount = p.Replies.Count(r => !r.IsDeleted),
                ReactionCounts = p.Reactions
                    .GroupBy(r => r.Type)
                    .Select(rt => new ReactionCount(rt.Key, rt.Count()))
            });

        var paged = await commentsQuery.ToPagedFeed(request.Filter);

        return Result.Success(paged);
    }
}
