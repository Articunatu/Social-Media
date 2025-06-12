using SM.Application.Abstractions;
using SM.Domain.Reactions;

namespace SM.Application.Reactions.UpdateReaction;

public record UpdateReactionCommand(ReactionType Type, Guid Id) : ICommand<ReactionResponse>;
