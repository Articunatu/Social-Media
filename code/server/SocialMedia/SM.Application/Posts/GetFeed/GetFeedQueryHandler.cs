using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;
using SM.Domain.Users.Extensions;

namespace SM.Application.Posts.GetFeed;

internal class GetFeedQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : IQueryHandler<GetFeedQuery, PagedFeed<FeedResponse>>
{
    public async Task<Result<PagedFeed<FeedResponse>>> Handle(GetFeedQuery request, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var followingIds = await context.Users
            .AsNoTracking()
            .Where(u => u.Id == request.UserId)
            .SelectMany(u => u.Following.Select(f => f.Id))
            .ToArrayAsync(cancellationToken: ct);

        if (followingIds.Length == 0)
            return Result.Success(new PagedFeed<FeedResponse> { Values = [] });

        var postsWithProfile = context.Posts
            .Where(p => followingIds.Contains(p.AuthorId))
            .Include(p => p.Author)
            .Select(p => new FeedResponse(
                p.Author.MapToProfile(),
                new ProfilePostDto
                {
                    PostId = p.Id,
                    Content = p.Content,
                    TimeStamp = p.TimeStamp,
                    CommentsCount = p.Comments.Count(),
                    ReactionCounts = p.Reactions
                        .GroupBy(r => r.Type)
                        .Select(rt => new ReactionCount(rt.Key, rt.Count()))
                }
            ))
            .AsQueryable();

        var filter = request.Filter with { Order = "TimeStamp desc" };

        var feed = await postsWithProfile.ToPagedFeed(filter);

        return Result.Success(feed);
    }
}
