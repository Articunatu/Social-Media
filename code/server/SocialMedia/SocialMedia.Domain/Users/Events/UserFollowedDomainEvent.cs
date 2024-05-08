using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Domain.Users.Events
{
    public record UserFollowedDomainEvent(Guid UserId) : IDomainEvent;
}
