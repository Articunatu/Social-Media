using SM.Application.Abstractions;
using SM.Application.Shared.Models;

namespace SM.Application.Profiles.GetProfilePosts;

public sealed record ProfileFeedResponse(PagedFeed<ProfilePostDto> ProfileFeed);