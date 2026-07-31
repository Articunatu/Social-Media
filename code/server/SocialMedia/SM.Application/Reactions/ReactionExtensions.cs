using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Content;

namespace SM.Application.Reactions;

public static class ReactionExtensions
{
    public static ReactionResponse MapToResponse(this Reaction reaction)
    {
        var profile = reaction.User is not null
            ? reaction.User.MapToProfile()
            : new ProfileInfo(reaction.UserId, string.Empty, string.Empty, null);

        return new ReactionResponse(reaction.Id, reaction.Type, profile);
    }
}
