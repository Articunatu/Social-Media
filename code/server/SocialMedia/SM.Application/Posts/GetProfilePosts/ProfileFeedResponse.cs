using SM.Application.Abstractions;
using SM.Application.Shared.Models;

namespace SM.Application.Posts.GetProfilePosts;

public sealed record ProfileFeedResponse(PagedFeed<ProfilePostDto> ProfileFeed);