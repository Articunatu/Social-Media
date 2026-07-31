using SM.Domain.Content;

namespace SM.Application.Shared.Models;

public class ReactionCount(ReactionType type, int amount)
{
    public ReactionType Type { get; set; } = type;
    public int Amount { get; set; } = amount;
}
