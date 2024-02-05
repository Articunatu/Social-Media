
using SocialMedia.Domain.Reactions;

namespace SocialMedia.Domain.Messages
{
    public sealed class Reply : Message
    {
        public Guid ParentPostId { get; set; }
        public ICollection<Reaction>? Reactions { get; set; }

    }
}
