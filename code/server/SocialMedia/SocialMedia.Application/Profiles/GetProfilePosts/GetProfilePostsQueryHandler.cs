using SocialMedia.Domain.Shared;
using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Profiles.GetProfilePosts;

internal class GetProfilePostsQueryHandler(IProfileRepository profileRepository)
        : IQueryHandler<GetProfilePostsQuery, ProfileFeedResponse>
{
    public async Task<Result<ProfileFeedResponse>> Handle(GetProfilePostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await profileRepository.GetPagedProfilePosts(request.UserId, request.Filter);

        if (posts?.ProfileFeed?.Values is not { } values || !values.Any())
            return Result.Failure<ProfileFeedResponse>(new Error("This user hasn't posted anything"));

        return Result.Success(posts);
    }
}
