using SocialMedia.Domain.Reactions;

namespace SocialMedia.Domain.Messages
{
    public abstract class Post : Message
    {
        public ICollection<Reaction>? Reactions { get; set; }
    }

    public sealed class PostRelational : Post
    {
        public Guid UserId { get; set; }
        public ICollection<Reply>? Replys { get; set; }
    }

    public sealed class PostNoSql : Post { }
}
