using SM.Domain.Shared;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Models;

namespace SM.Application.Profiles.GetProfilePosts;

internal class GetProfilePostsQueryHandler(ApplicationDbContext context)
        : IQueryHandler<GetProfilePostsQuery, ProfileFeedResponse>
{
    public async Task<Result<ProfileFeedResponse>> Handle(GetProfilePostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var posts = await context.Posts
                .Where(p => p.AuthorId == request.UserId)
                .Select(p => new ProfilePostDto
                {
                    Content = p.Content,
                    TimeStamp = DateTime.Now,
                    //ReactionCounts = p.Reactions.GroupBy(r => r.Type),
                    RepliesCount = p.Replys.Count(),
                })
                .AsQueryable()
                .ToPagedFeed(request.Filter);
        }
        catch (Exception)
        {
            return Result.Failure<ProfileFeedResponse>(new Error("This user id doesnt exist"));
        }

        if (posts?.ProfileFeed?.Values is not { } values || !values.Any())
            return Result.Failure<ProfileFeedResponse>(new Error("This user hasn't posted anything"));

        return Result.Success(posts);
    }
}
