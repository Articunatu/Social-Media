using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Reactions.GetReactedPostsByUser;

internal class GetReactedPostsByUserQueryHandler(IDbContextFactory<ContentDbContext> contentContextFactory)
    : IQueryHandler<GetReactedPostsByUserQuery, PagedFeed<ReactedProfilePost>>
{
    public async Task<Result<PagedFeed<ReactedProfilePost>>> Handle(GetReactedPostsByUserQuery request, CancellationToken cancellationToken)
    {
        await using var contentContext = await contentContextFactory.CreateDbContextAsync(cancellationToken);

        var reactedPostsQuery = contentContext.Reactions
            .AsNoTracking()
            .Where(r => r.UserId == request.UserId && r.PostId != null)
            .Select(r => new ReactedProfilePost(r.Id, r.Type)
            {
                PostId = r.PostId!.Value,
                Content = r.Post!.Content,
                TimeStamp = r.Post.TimeStamp,
                CommentsCount = contentContext.Comments.Count(c => c.ParentPostId == r.PostId && c.ParentCommentId == null),
                ReactionCounts = contentContext.Reactions
                    .Where(x => x.PostId == r.PostId)
                    .GroupBy(x => x.Type)
                    .Select(g => new ReactionCount(g.Key, g.Count()))
                    .ToList()
            });
        
        var filter = new PageFilter { Index = request.PagingIndex };

        var pagedReactions = await reactedPostsQuery.ToPagedFeed(filter);

        return Result.Success(pagedReactions);
    }
}
