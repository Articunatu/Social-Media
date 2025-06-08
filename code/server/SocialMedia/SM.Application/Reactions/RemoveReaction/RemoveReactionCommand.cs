using SM.Application.Abstractions;

namespace SM.Application.Reactions.RemoveReaction;

public record RemoveReactionCommand(Guid Id) : ICommand<ReactionResponse>;