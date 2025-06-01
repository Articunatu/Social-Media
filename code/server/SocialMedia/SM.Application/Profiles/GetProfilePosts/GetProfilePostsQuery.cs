using SM.Application.Abstractions;
using SM.Application.Shared.Models;

namespace SM.Application.Profiles.GetProfilePosts;

public record GetProfilePostsQuery(Guid UserId, PageFilter Filter) : IQuery<ProfileFeedResponse>;
