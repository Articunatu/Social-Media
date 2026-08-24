using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;
using SM.Domain.Users.Extensions;

namespace SM.Application.Posts.GetFeed;

internal class GetFeedQueryHandler(
    IDbContextFactory<FeedDbContext> feedContextFactory,
    IDbContextFactory<ContentDbContext> contentContextFactory,
    IDbContextFactory<IdentityDbContext> identityContextFactory)
    : IQueryHandler<GetFeedQuery, PagedFeed<FeedResponse>>
{
    public async Task<Result<PagedFeed<FeedResponse>>> Handle(GetFeedQuery request, CancellationToken ct)
    {
        await using var feedContext = await feedContextFactory.CreateDbContextAsync(ct);
        await using var contentContext = await contentContextFactory.CreateDbContextAsync(ct);
        await using var identityContext = await identityContextFactory.CreateDbContextAsync(ct);

        var feedItems = feedContext.FeedItems
            .AsNoTracking()
            .Where(item => item.RecipientId == request.UserId);

        var pagedItems = await feedItems
            .Select(item => new
            {
                item.PostId,
                item.AuthorId,
                item.CreatedAt
            })
            .ToPagedFeed(request.Filter with { Order = "CreatedAt desc" });

        if (!pagedItems.Values.Any())
            return Result.Success(new PagedFeed<FeedResponse> { Values = [] });

        var postIds = pagedItems.Values.Select(item => item.PostId).ToArray();
        var posts = await contentContext.Posts
            .Where(post => postIds.Contains(post.Id))
            .Select(p => new
            {
                p.Id,
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
            .ToDictionaryAsync(post => post.Id, cancellationToken: ct);

        var profiles = await identityContext.GetProfileLookupAsync(pagedItems.Values.Select(x => x.AuthorId), ct);

        return Result.Success(new PagedFeed<FeedResponse>
        {
            Index = pagedItems.Index,
            Order = pagedItems.Order,
            SearchText = pagedItems.SearchText,
            Values = pagedItems.Values
                .Where(item => posts.ContainsKey(item.PostId))
                .Select(item => new FeedResponse(
                    profiles.TryGetValue(item.AuthorId, out var profile)
                        ? profile
                        : new ProfileInfo(item.AuthorId, string.Empty, string.Empty, null),
                    posts[item.PostId].Post))
        });
    }
}
