using SM.Application.Abstractions;

namespace SM.Application.Profiles.GetProfilePosts;

public record GetProfilePostsQuery(Guid UserId, PageFilter Filter) : IQuery<ProfileFeedResponse>;
