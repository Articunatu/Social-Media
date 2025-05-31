using SM.Domain.Reactions;

namespace SM.Application.Shared.Models;

public record ProfilePostDto
{
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset TimeStamp { get; set; }
    public int RepliesCount { get; set; }
    public IEnumerable<ReactionCount> ReactionCounts { get; set; } = [];
}

