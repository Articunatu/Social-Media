using SocialMedia.Domain.Reactions;

namespace SocialMedia.Domain.Messages
{
    public sealed class Reply : Post
    {
        public Guid ParentPostId { get; set; }
    }
}
