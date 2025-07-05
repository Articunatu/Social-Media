using SM.Application.Abstractions;

namespace SM.Application.Users.Unfollow;

public record UnfollowCommand(Guid FollowerId, Guid FollowingId) 
    : ICommand<IEnumerable<UserCommandResponse>>;
