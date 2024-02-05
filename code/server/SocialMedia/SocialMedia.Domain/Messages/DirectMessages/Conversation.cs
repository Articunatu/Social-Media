using SocialMedia.Domain.Users;

namespace SocialMedia.Domain.Messages.DirectMessages
{
    public sealed class Conversation
    {
        public Guid Id { get; set; }
        public ICollection<DirectMessage> SendersMessages { get; set; }
        public ICollection<DirectMessage>? ReceiversMessages { get; set; }
    }
}
