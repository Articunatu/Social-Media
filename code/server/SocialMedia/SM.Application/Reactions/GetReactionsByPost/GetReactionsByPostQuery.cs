using SM.Application.Abstractions;
using SM.Application.Shared.Models;
using SM.Domain.Content;

namespace SM.Application.Reactions.GetReactionsByPost;

public record GetReactionsByPostQuery(Guid PostId, ReactionType? Type, PageFilter Filter) : IQuery<PagedFeed<ReactionResponse>>;