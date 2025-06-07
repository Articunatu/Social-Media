using SM.Application.Abstractions;
using SM.Application.Shared.Models;

namespace SM.Application.Reactions.GetReactedPostsByUser;

public record GetReactedPostsByUserQuery(Guid UserId) : IQuery<PagedFeed<ProfilePostDto>>;
