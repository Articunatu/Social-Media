using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Shared;
using SM.Domain.Users;
using System.Net;

namespace SM.Application.Users.Follow;

internal class FollowCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    : ICommandHandler<FollowCommand, IEnumerable<UserCommandResponse>>
{
    public async Task<Result<IEnumerable<UserCommandResponse>>> Handle(FollowCommand request, CancellationToken cancellationToken)
    {
        if (request.FollowerId == request.FollowingId)
            return Result.Failure<IEnumerable<UserCommandResponse>>(new Error("User.InvalidOperation", "A user cannot follow themself."), HttpStatusCode.BadRequest);

        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var follower = await context.Users.Include(u => u.Following).FirstOrDefaultAsync(u => u.Id == request.FollowerId, cancellationToken);
        var following = await context.Users.Include(u => u.Followers).FirstOrDefaultAsync(u => u.Id == request.FollowingId, cancellationToken);

        if (follower is null || following is null)
            return Result.Failure<IEnumerable<UserCommandResponse>>(UserErrors.NotFound, HttpStatusCode.NotFound);

        if (follower.Following.Any(f => f.Id == following.Id))
            return Result.Failure<IEnumerable<UserCommandResponse>>(new Error("User.AlreadyFollowing", "The follower is already following the specified user."), HttpStatusCode.BadRequest);

        follower.Following.Add(following);
        following.Followers.Add(follower);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success<IEnumerable<UserCommandResponse>>(new[]
        {
            follower.MapToCommandResponse(),
            following.MapToCommandResponse()
        });
    }
}
