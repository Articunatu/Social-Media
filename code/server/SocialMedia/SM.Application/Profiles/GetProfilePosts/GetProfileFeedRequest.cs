using SM.Application.Shared.Models;

namespace SM.Application.Profiles.GetProfilePosts;

public record GetProfileFeedRequest(Guid UserId, PageFilter Filter);