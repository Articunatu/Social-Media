using SM.Application.Abstractions;

namespace SM.Application.Users.Follow;

public record FollowCommand(Guid FollowerId, Guid FollowingId) : ICommand<IEnumerable<UserCommandResponse>>;
