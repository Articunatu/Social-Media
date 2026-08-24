using SM.Domain.Abstractions;

namespace SM.Domain.Messaging;

public sealed class Conversation(Guid id) : Entity<Guid>(id)
{
    public ICollection<DirectMessage> Messages { get; private set; } = [];

    public static Conversation Create() => new(Guid.CreateVersion7());
}
