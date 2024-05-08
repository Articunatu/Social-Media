using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Users.FollowUsers
{
    public sealed record FollowUserCommand(
        Guid FollowerId,
        Guid FollowingId,
        bool isUnfollow
        ) : ICommand<Guid>;
}
