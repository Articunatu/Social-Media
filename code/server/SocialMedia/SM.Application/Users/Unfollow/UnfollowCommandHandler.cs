using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Shared;
using SM.Domain.Users;
using System.Net;

namespace SM.Application.Users.Unfollow;

internal class UnfollowCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    : ICommandHandler<UnfollowCommand, IEnumerable<UserCommandResponse>>
{
    public async Task<Result<IEnumerable<UserCommandResponse>>> Handle(UnfollowCommand request, CancellationToken cancellationToken)
    {
        if (request.FollowerId == request.FollowingId)
            return Result.Failure<IEnumerable<UserCommandResponse>>(new Error("User.InvalidOperation", "A user cannot unfollow themself."), HttpStatusCode.BadRequest);

        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var follower = await context.Users.Include(u => u.Following).FirstOrDefaultAsync(u => u.Id == request.FollowerId, cancellationToken);
        var following = await context.Users.Include(u => u.Followers).FirstOrDefaultAsync(u => u.Id == request.FollowingId, cancellationToken);

        if (follower is null || following is null)
            return Result.Failure<IEnumerable<UserCommandResponse>>(UserErrors.NotFound, HttpStatusCode.NotFound);

        if (!follower.Following.Any(f => f.Id == following.Id))
            return Result.Failure<IEnumerable<UserCommandResponse>>(new Error("User.NotFollowing", "The follower is not following the specified user."), HttpStatusCode.BadRequest);

        follower.Following.Remove(following);
        following.Followers.Remove(follower);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success<IEnumerable<UserCommandResponse>>(new[]
        {
            follower.MapToCommandResponse(),
            following.MapToCommandResponse()
        });
    }
}
