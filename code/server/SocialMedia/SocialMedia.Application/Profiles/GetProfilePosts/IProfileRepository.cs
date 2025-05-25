using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Profiles.GetProfilePosts;

public interface IProfileRepository
{
    public Task<ProfileFeedResponse?> GetPagedProfilePosts(Guid userId, PageFilter filter);
}
