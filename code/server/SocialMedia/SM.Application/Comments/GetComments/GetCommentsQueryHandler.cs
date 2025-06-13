using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Comments.GetComments;

internal class GetCommentsQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : IQueryHandler<GetCommentsQuery, PagedFeed<CommentResponse>>
{
    public async Task<Result<PagedFeed<CommentResponse>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var comments = await context.Comments
            .Where(p => p.ParentPostId == request.Id)
            .Select(p => new CommentQuery
            {
                ParentPostId = request.Id,
                Content = p.Content,
                TimeStamp = p.TimeStamp,
                CommentsCount = p.Comments != null ? p.Comments.Count() : 0,
                ReactionCounts = p.Reactions != null
                    ? p.Reactions
                        .GroupBy(r => r.Type)
                        .Select(rt => new ReactionCount(rt.Key, rt.Count()))
                    : new List<ReactionCount>()
            })
            .AsQueryable()
            .ToPagedFeed(request.Filter);
    }
}
