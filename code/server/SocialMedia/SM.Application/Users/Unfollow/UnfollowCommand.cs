using SM.Application.Abstractions;
using SM.Application.Users.Follow;

namespace SM.Application.Users.Unfollow;

public record UnfollowCommand(Guid FollowerId, Guid FollowingId) 
    : FollowCommand(FollowerId, FollowingId), ICommand<IEnumerable<UserCommandResponse>>;
