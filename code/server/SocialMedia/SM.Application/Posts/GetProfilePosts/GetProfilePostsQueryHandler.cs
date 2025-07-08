using SM.Domain.Shared;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Models;
using SM.Application.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace SM.Application.Posts.GetProfilePosts;

internal class GetProfilePostsQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    : IQueryHandler<GetProfilePostsQuery, ProfileFeedResponse>
{
    public async Task<Result<ProfileFeedResponse>> Handle(GetProfilePostsQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var emptyReactionCounts = new List<ReactionCount>();

        var posts = await context.Posts
            .Where(p => p.AuthorId == request.UserId)
            .Select(p => new ProfilePostDto
            {
                Content = p.Content,
                TimeStamp = p.TimeStamp,
                CommentsCount = p.Comments != null ? p.Comments.Count() : 0,
                ReactionCounts = p.Reactions != null
                    ? p.Reactions.GroupBy(r => r.Type)
                        .Select(rt => new ReactionCount(rt.Key, rt.Count()))
                    : emptyReactionCounts
            })
            .ToPagedFeed(request.Filter);

        if (posts.Values is not { } values || !values.Any())
        {
            return Result.Failure<ProfileFeedResponse>(
                new Error("This user hasn't posted anything"), StatusCode.NoContent);
        }

        return Result.Success(new ProfileFeedResponse(posts));
    }
}
