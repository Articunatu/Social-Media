using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Domain.Users.Events
{
    public record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;
}
