using SM.Application.Shared.Extensions;
using SM.Domain.Reactions;

namespace SM.Application.Reactions;

public static class ReactionExtensions
{
    public static ReactionResponse MapToResponse(this Reaction reaction)
    {
        return new ReactionResponse(reaction.Id, reaction.Type, reaction.User.MapToProfile());
    }
}
