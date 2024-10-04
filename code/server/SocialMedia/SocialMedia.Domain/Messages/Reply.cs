using SocialMedia.Domain.Reactions;

namespace SocialMedia.Domain.Messages
{
    public abstract class Reply : Post
    {
        public Guid ParentPostId { get; set; }
        public ICollection<Reaction>? Reactions { get; set; }
    }

    public sealed class ReplyRelational : Reply
    {
        public Guid UserId { get; set; }
    }

    public sealed class ReplyNoSql : Post { }
}
