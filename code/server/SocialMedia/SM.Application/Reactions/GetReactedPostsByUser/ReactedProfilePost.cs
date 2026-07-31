using SM.Application.Shared.Models;
using SM.Domain.Content;

namespace SM.Application.Reactions.GetReactedPostsByUser;

public record ReactedProfilePost(Guid ReactionId, ReactionType Type) : ProfilePostDto;