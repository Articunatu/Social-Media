using SM.Application.Shared.Models;
using SM.Domain.Content;

namespace SM.Application.Reactions;

public record ReactionResponse
(
    Guid Id,
    ReactionType Type,
    ProfileInfo Profile
);
