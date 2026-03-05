using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Posts.GetFeed;

internal class GetFeedQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : IQueryHandler<GetFeedQuery, PagedFeed<FeedResponse>>
{
    public async Task<Result<PagedFeed<FeedResponse>>> Handle(GetFeedQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var followingIds = await context.Users
            .Where(u => u.Id == request.UserId)
            .SelectMany(u => u.Following.Select(f => f.Id))
            .ToArrayAsync(cancellationToken: cancellationToken);

        if (followingIds.Length == 0)
            return Result.Success(new PagedFeed<FeedResponse> { Values = [] });

        IEnumerable<ReactionCount> emptyReactionCounts = [];

        var postsWithProfile = context.Posts
            .Where(p => followingIds.Contains(p.AuthorId))
            .Include(p => p.Author)
            .Select(p => new FeedResponse(
                new ProfileInfo(p.Author.Id, p.Author.Tag, p.Author.FirstName + " " + p.Author.LastName, p.Author.GetProfilePhoto()),
                new ProfilePostDto
                {
                    Content = p.Content,
                    TimeStamp = p.TimeStamp,
                    CommentsCount = p.Comments != null ? p.Comments.Count() : 0,
                    ReactionCounts = p.Reactions != null
                        ? p.Reactions
                            .GroupBy(r => r.Type)
                            .Select(rt => new ReactionCount(rt.Key, rt.Count()))
                            .ToList()
                        : emptyReactionCounts
                }
            ))
            .AsQueryable();

        request.Filter.Order = "TimeStamp desc";

        var feed = await postsWithProfile.ToPagedFeed(request.Filter);

        return Result.Success(feed);
    }
}
