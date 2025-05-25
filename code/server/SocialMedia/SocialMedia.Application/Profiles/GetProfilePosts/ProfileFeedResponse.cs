using SocialMedia.Application.Abstractions;
using SocialMedia.Application.Shared.Models;

namespace SocialMedia.Application.Profiles.GetProfilePosts;

public sealed record ProfileFeedResponse(ProfileInfo UserProfile, PagedFeed<ProfilePostDto> ProfileFeed);