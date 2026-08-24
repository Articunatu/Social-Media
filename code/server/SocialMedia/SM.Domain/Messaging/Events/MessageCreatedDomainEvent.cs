using SM.Domain.Abstractions;

namespace SM.Domain.Messaging.Events;

public record MessageCreatedDomainEvent(
	Guid MessageId,
	Guid ConversationId,
	Guid AuthorId,
	DateTimeOffset CreatedAt) : IDomainEvent;
