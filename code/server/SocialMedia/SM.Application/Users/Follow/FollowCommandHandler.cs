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

        var follower = await context.Users.FirstOrDefaultAsync(u => u.Id == request.FollowerId, cancellationToken);
        var following = await context.Users.FirstOrDefaultAsync(u => u.Id == request.FollowingId, cancellationToken);

        if (follower is null || following is null)
            return Result.Failure<IEnumerable<UserCommandResponse>>(UserErrors.NotFound, HttpStatusCode.NotFound);

        var alreadyFollowing = await context.Follows.AnyAsync(
            f => f.FollowerId == request.FollowerId && f.FollowingId == request.FollowingId, cancellationToken);

        if (alreadyFollowing)
            return Result.Failure<IEnumerable<UserCommandResponse>>(new Error("User.AlreadyFollowing", "The follower is already following the specified user."), HttpStatusCode.BadRequest);

        context.Follows.Add(SM.Domain.SocialGraph.Follow.Create(request.FollowerId, request.FollowingId));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success<IEnumerable<UserCommandResponse>>(new[]
        {
            follower.MapToCommandResponse(),
            following.MapToCommandResponse()
        });
    }
}
