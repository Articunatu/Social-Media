using SM.Application.Abstractions;
using SM.Domain.Reactions;

namespace SM.Application.Reactions.AddReaction;

public record AddReactionCommand(ReactionType Type, Guid UserId, Guid MessageId) : ICommand<ReactionResponse>;
