using SM.Application.Shared.Models;
using SM.Domain.Reactions;

namespace SM.WebApi.Endpoints;

public class GetReactionsByPostRequest
{
    public ReactionType? Type { get; set; }
    public PageFilter Filter { get; set; } = new();
}