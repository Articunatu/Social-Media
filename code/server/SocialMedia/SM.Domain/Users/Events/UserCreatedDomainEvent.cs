using SM.Domain.Abstractions;

namespace SM.Domain.Users.Events
{
    public record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;
}
