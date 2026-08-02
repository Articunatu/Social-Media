using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;
using SM.Domain.Users.Extensions;

namespace SM.Application.Posts.GetFeed;

internal class GetFeedQueryHandler(
    IDbContextFactory<SocialGraphDbContext> socialGraphContextFactory,
    IDbContextFactory<ContentDbContext> contentContextFactory,
    IDbContextFactory<IdentityDbContext> identityContextFactory)
    : IQueryHandler<GetFeedQuery, PagedFeed<FeedResponse>>
{
    public async Task<Result<PagedFeed<FeedResponse>>> Handle(GetFeedQuery request, CancellationToken ct)
    {
        await using var socialGraphContext = await socialGraphContextFactory.CreateDbContextAsync(ct);
        await using var contentContext = await contentContextFactory.CreateDbContextAsync(ct);
        await using var identityContext = await identityContextFactory.CreateDbContextAsync(ct);

        var followingIds = await socialGraphContext.Follows
            .AsNoTracking()
            .Where(f => f.FollowerId == request.UserId)
            .Select(f => f.FollowingId)
            .ToArrayAsync(cancellationToken: ct);

        if (followingIds.Length == 0)
            return Result.Success(new PagedFeed<FeedResponse> { Values = [] });

        var pagedPosts = await contentContext.Posts
            .Where(p => followingIds.Contains(p.AuthorId))
            .Select(p => new
            {
                p.AuthorId,
                Post = new ProfilePostDto
                {
                    PostId = p.Id,
                    Content = p.Content,
                    TimeStamp = p.TimeStamp,
                    CommentsCount = p.Comments.Count(),
                    ReactionCounts = p.Reactions
                        .GroupBy(r => r.Type)
                        .Select(rt => new ReactionCount(rt.Key, rt.Count()))
                }
            })
            .ToPagedFeed(request.Filter with { Order = "TimeStamp desc" });

        var profiles = await identityContext.GetProfileLookupAsync(pagedPosts.Values.Select(x => x.AuthorId), ct);

        return Result.Success(new PagedFeed<FeedResponse>
        {
            Index = pagedPosts.Index,
            Order = pagedPosts.Order,
            SearchText = pagedPosts.SearchText,
            Values = pagedPosts.Values.Select(x => new FeedResponse(
                profiles.TryGetValue(x.AuthorId, out var profile)
                    ? profile
                    : new ProfileInfo(x.AuthorId, string.Empty, string.Empty, null),
                x.Post))
        });
    }
}
