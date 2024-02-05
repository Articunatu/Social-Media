using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Domain.Messages
{
    public class Message : Entity<Guid>
    {
        public Message() { }

        public Message(Guid id, string content, DateTime timeStamp) : base(id)
        {
            Content = content;
            TimeStamp = timeStamp;
        }

        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid UserId { get; set; }
    }
}
