using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Shared;
using SM.Domain.Users;

namespace SM.Application.Users.Unfollow;

internal class UnfollowCommandHandler(ApplicationDbContext context) 
    : ICommandHandler<UnfollowCommand, IEnumerable<UserCommandResponse>>
{
    public async Task<Result<IEnumerable<UserCommandResponse>>> Handle(UnfollowCommand request, CancellationToken cancellationToken)
    {
        var followerTask = context.Users.FirstOrDefaultAsync(u => u.Id == request.FollowerId, cancellationToken);
        var followingTask = context.Users.FirstOrDefaultAsync(u => u.Id == request.FollowingId, cancellationToken);

        await Task.WhenAll(followerTask, followingTask);

        var follower = await followerTask;
        var following = await followingTask;

        if (follower is null || following is null)
            return Result.Failure<IEnumerable<UserCommandResponse>>(UserErrors.NotFound);

        follower.Following.Remove(following);
        following.Followers.Remove(follower);

        await context.SaveChangesAsync(cancellationToken);

        IEnumerable<UserCommandResponse> response =
        [
            follower.MapToCommandResponse(),
            following.MapToCommandResponse()
        ];

        return Result.Success(response);
    }
}
