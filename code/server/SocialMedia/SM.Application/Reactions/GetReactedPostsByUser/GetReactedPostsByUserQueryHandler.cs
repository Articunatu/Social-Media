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

        var reactedPosts = context.Reactions
            .Where(r => r.UserId == request.UserId)
            .Include(r => r.Message)
            .ThenInclude(m => m.Author)
            .Select(r => new ReactedProfilePost(r.Type)
            {
                Content = r.Message.Content,
                TimeStamp = r.Message.TimeStamp,
                CommentsCount = 4, //r.Message.Comments != null ? r.Message.Replies.Count() : 0,
                ReactionCounts = r.Message.Reactions != null
                        ? r.Message.Reactions
                            .GroupBy(r => r.Type)
                            .Select(rt => new ReactionCount(rt.Key, rt.Count()))
                        : new List<ReactionCount>()
            }).AsQueryable();

        var filter = new PageFilter()
        {
            Index = request.PagingIndex
        };

        var pagedReactions = await reactedPosts.ToPagedFeed(filter);

        return Result.Success(pagedReactions);
    }
}
