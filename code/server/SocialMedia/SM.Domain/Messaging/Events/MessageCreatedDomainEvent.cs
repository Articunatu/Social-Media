using SM.Domain.Abstractions;

namespace SM.Domain.Messaging.Events;

public record MessageCreatedDomainEvent(Guid Message) : IDomainEvent;
