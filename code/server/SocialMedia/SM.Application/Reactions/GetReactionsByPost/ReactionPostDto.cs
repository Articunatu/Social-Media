using SM.Application.Shared.Models;
using SM.Domain.Reactions;

namespace SM.Application.Reactions.GetReactionsByPost;

public record ReactionPostDto(Guid Id, ReactionType Type, ProfileInfo Profile, ProfilePostDto Post) 
    : ReactionResponse(Id, Type, Profile);
