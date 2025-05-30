using SM.Domain.Abstractions;

namespace SM.Domain.Users.Events
{
    public record UserFollowedDomainEvent(Guid UserId) : IDomainEvent;
}
