using SM.Domain.Reactions;

namespace SM.Domain.Messages;

public class Post : Message
{
    public ICollection<Reply> Replies { get; set; } = [];
    public ICollection<Reaction> Reactions { get; set; } = [];
}
