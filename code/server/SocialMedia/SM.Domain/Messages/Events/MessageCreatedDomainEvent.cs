using SM.Domain.Abstractions;

namespace SM.Domain.Messages.Events
{
    public record MessageCreatedDomainEvent(Guid Message) : IDomainEvent;
}
