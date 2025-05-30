using SM.Application.Abstractions;

namespace SM.Application.Profiles.GetProfilePosts;

public record GetProfileFeedRequest(Guid UserId, PageFilter Filter);