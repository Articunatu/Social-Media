using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Profiles.GetProfilePosts;

public record GetProfilePostsQuery(Guid UserId, PageFilter Filter) : IQuery<ProfileFeedResponse>;
