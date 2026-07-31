using SM.Application.Abstractions;
using SM.Domain.Content;

namespace SM.Application.Reactions.AddReaction;

public record AddReactionCommand(ReactionType Type, Guid UserId, Guid MessageId) : ICommand<ReactionResponse>;
