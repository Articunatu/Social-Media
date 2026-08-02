using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Behaviors;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Shared;
using SM.Domain.SocialGraph;
using SM.Domain.Users;
using System.Net;

namespace SM.Application.Users.Unfollow;

internal class UnfollowCommandHandler(
    IDbContextFactory<IdentityDbContext> identityContextFactory,
    IDbContextFactory<SocialGraphDbContext> socialGraphContextFactory,
    ILoggingBehaviour logging)
    : ICommandHandler<UnfollowCommand, IEnumerable<UserCommandResponse>>
{
    public async Task<Result<IEnumerable<UserCommandResponse>>> Handle(UnfollowCommand request, CancellationToken cancellationToken)
    {
        if (request.FollowerId == request.FollowingId)
            return Result.Failure<IEnumerable<UserCommandResponse>>(new Error("User.InvalidOperation", "A user cannot unfollow themself."), HttpStatusCode.BadRequest);

        await using var identityContext = await identityContextFactory.CreateDbContextAsync(cancellationToken);
        await using var context = await socialGraphContextFactory.CreateDbContextAsync(cancellationToken);

        var follower = await identityContext.Users.FirstOrDefaultAsync(u => u.Id == request.FollowerId, cancellationToken);
        var following = await identityContext.Users.FirstOrDefaultAsync(u => u.Id == request.FollowingId, cancellationToken);

        if (follower is null || following is null)
            return Result.Failure<IEnumerable<UserCommandResponse>>(UserErrors.NotFound, HttpStatusCode.NotFound);

        var follow = await context.Follows.FirstOrDefaultAsync(
            f => f.FollowerId == request.FollowerId && f.FollowingId == request.FollowingId, cancellationToken);

        if (follow is null)
            return Result.Failure<IEnumerable<UserCommandResponse>>(new Error("User.NotFollowing", "The follower is not following the specified user."), HttpStatusCode.BadRequest);

        context.Follows.Remove(follow);

        try
        {
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success<IEnumerable<UserCommandResponse>>(
            [
                follower.MapToCommandResponse(),
                following.MapToCommandResponse()
            ]);
        }
        catch (Exception ex)
        {
            logging.LogError($"An error occurred while saving changes to the database during unfollow operation. FollowerId: {request.FollowerId}, FollowingId: {request.FollowingId}", ex);
            return Result.Failure<IEnumerable<UserCommandResponse>>(new Error("DatabaseError", "An error occurred while saving changes to the database."), HttpStatusCode.InternalServerError);
        }
    }
}
