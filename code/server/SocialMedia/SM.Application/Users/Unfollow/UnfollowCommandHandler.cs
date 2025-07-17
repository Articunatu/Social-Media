using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Shared;
using SM.Domain.Users;

namespace SM.Application.Users.Unfollow;

internal class UnfollowCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    : ICommandHandler<UnfollowCommand, IEnumerable<UserCommandResponse>>
{
    public async Task<Result<IEnumerable<UserCommandResponse>>> Handle(UnfollowCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var follower = await context.Users.FirstOrDefaultAsync(u => u.Id == request.FollowerId, cancellationToken);
        var following = await context.Users.FirstOrDefaultAsync(u => u.Id == request.FollowingId, cancellationToken);

        if (follower is null || following is null)
            return Result.Failure<IEnumerable<UserCommandResponse>>(UserErrors.NotFound, StatusCode.NotFound);

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
