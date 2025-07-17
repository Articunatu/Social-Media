using SM.Application.Abstractions;

namespace SM.Application.Reactions.GetReactedPostsByUser;

public record GetReactedPostsByUserQuery(Guid UserId, int PagingIndex) : IQuery<PagedFeed<ReactedProfilePost>>;
