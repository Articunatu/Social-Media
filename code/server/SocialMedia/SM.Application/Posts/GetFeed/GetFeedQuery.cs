using SM.Application.Abstractions;
using SM.Application.Shared.Models;

namespace SM.Application.Posts.GetFeed;

public record GetFeedQuery(Guid UserId, PageFilter Filter) : IQuery<PagedFeed<FeedResponse>>;
