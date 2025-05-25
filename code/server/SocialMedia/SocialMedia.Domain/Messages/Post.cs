using SocialMedia.Domain.Reactions;

namespace SocialMedia.Domain.Messages;

public class Post : Message
{
    public Guid AuthorId { get; set; }
    public ICollection<Reply> Replys { get; set; } = [];
    public ICollection<Reaction> Reactions { get; set; } = [];
}
