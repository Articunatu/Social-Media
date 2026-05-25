using SM.Application.Abstractions;

namespace SM.Application.Reactions.GetMyReactionByPost;

public record GetMyReactionByPostQuery(Guid UserId, Guid PostId) : IQuery<ReactionResponse>;
