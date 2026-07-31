namespace SM.Domain.Messaging;

public sealed class Conversation
{
    public Guid Id { get; set; }
    public ICollection<DirectMessage> SendersMessages { get; set; } = [];
    public ICollection<DirectMessage> ReceiversMessages { get; set; } = [];
}
