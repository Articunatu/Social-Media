using SM.Application.Shared.Models;

namespace SM.Application.Posts.GetFeed;

public record FeedResponse(ProfileInfo Profile, ProfilePostDto Post);
