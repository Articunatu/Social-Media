using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Profiles.GetProfilePosts;

public record GetProfileFeedRequest(Guid UserId, PageFilter Filter);