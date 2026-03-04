
namespace SM.Application.Shared.Models;

public record ProfilePostDto
{
    public string Content { get; init; } = string.Empty;
    public DateTimeOffset TimeStamp { get; init; }
    public int CommentsCount { get; init; }
    public IEnumerable<ReactionCount>? ReactionCounts { get; init; }
}

