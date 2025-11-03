using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Reactions.GetReactedPostsByUser;

internal class GetReactedPostsByUserQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    : IQueryHandler<GetReactedPostsByUserQuery, PagedFeed<ReactedProfilePost>>
{
    public async Task<Result<PagedFeed<ReactedProfilePost>>> Handle(GetReactedPostsByUserQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var reactedPostsQuery = context.Reactions
            .AsNoTracking()
            .Where(r => r.UserId == request.UserId && r.PostId != null && r.Post != null)
            .Include(r => r.Post)
                .ThenInclude(p => p.Author)
            .Select(r => new ReactedProfilePost(r.Type)
            {
                Content = r.Post!.Content,
                TimeStamp = r.Post.TimeStamp,
                CommentsCount = r.Post.Comments.Where(c => !c.IsDeleted && c.ParentCommentId == null).Count(),
                ReactionCounts = r.Post.Reactions
                    .GroupBy(x => x.Type)
                    .Select(g => new ReactionCount(g.Key, g.Count()))
            });

        var filter = new PageFilter { Index = request.PagingIndex };

        var pagedReactions = await reactedPostsQuery.ToPagedFeed(filter);

        return Result.Success(pagedReactions);
    }
}
