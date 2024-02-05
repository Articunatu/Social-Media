namespace SocialMedia.Domain.Messages.DirectMessages
{
    public sealed class DirectMessage : Message
    {
        public Conversation Conversation { get; set; }
    }
}
