using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Domain.Users.Events
{
    public record MessageCreatedDomainEvent(Guid Message) : IDomainEvent;
}
