using SM.Domain.Abstractions;

namespace SM.Domain.Messaging;

public sealed class Conversation(Guid id) : Entity<Guid>(id)
{
    public ICollection<DirectMessage> Messages { get; private set; } = [];
    public ICollection<ConversationParticipant> Participants { get; private set; } = [];

    public static Conversation Create(IEnumerable<Guid> participantIds)
    {
        ArgumentNullException.ThrowIfNull(participantIds);

        var distinctParticipantIds = participantIds.Distinct().ToArray();
        if (distinctParticipantIds.Length == 0)
            throw new ArgumentException("A conversation must have at least one participant.", nameof(participantIds));

        var conversation = new Conversation(Guid.CreateVersion7());
        foreach (var participantId in distinctParticipantIds)
            conversation.Participants.Add(new ConversationParticipant(conversation.Id, participantId));

        return conversation;
    }
}
