using SM.Application.Abstractions;
using SM.Application.Posts.GetProfilePosts;

namespace SM.Application.Posts.GetExploredPosts;

public record GetExploredPostsQuery() : IQuery<ProfileFeedResponse>;
