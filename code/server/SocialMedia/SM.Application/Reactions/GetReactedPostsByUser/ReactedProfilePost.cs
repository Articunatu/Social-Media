using SM.Application.Shared.Models;
using SM.Domain.Reactions;

namespace SM.Application.Reactions.GetReactedPostsByUser;

public record ReactedProfilePost(ReactionType Type) : ProfilePostDto;
