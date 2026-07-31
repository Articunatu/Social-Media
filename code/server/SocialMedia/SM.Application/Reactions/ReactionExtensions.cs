using SM.Application.Shared.Models;
using SM.Domain.Content;

namespace SM.Application.Reactions;

public static class ReactionExtensions
{
    public static ReactionResponse MapToResponse(this Reaction reaction, ProfileInfo profile)
    {
        return new ReactionResponse(reaction.Id, reaction.Type, profile);
    }
}
