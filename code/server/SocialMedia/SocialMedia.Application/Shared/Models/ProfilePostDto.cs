using SocialMedia.Domain.Reactions;

namespace SocialMedia.Application.Shared.Models;

public record ProfilePostDto
{
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset TimeStamp { get; set; }
    public int RepliesCount { get; set; }
    public Dictionary<ReactionType, int> ReactionCounts { get; set; } = [];
}

