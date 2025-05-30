using SM.Application.Abstractions;

namespace SM.Application.Profiles.GetProfilePosts;

public interface IProfileRepository
{
    public Task<ProfileFeedResponse?> GetPagedProfilePosts(Guid userId, PageFilter filter);
}
