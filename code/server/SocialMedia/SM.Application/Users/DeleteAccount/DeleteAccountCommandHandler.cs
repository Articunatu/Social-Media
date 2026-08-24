using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Behaviors;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Shared;
using SM.Domain.Users;
using System.Net;

namespace SM.Application.Users.DeleteAccount;

internal class DeleteAccountCommandHandler(IDbContextFactory<IdentityDbContext> contextFactory, IDbContextFactory<MediaDbContext> mediaContextFactory, ILoggingBehaviour logging) 
    : ICommandHandler<DeleteAccountCommand, UserCommandResponse>
{
    public async Task<Result<UserCommandResponse>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        await using var mediaContext = await mediaContextFactory.CreateDbContextAsync(cancellationToken);

        var user = await context.Users
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            return Result.Failure<UserCommandResponse>(UserErrors.NotFound, HttpStatusCode.NotFound);

        var photos = await mediaContext.Photos
            .Where(photo => photo.UserId == user.Id)
            .ToListAsync(cancellationToken);
        mediaContext.Photos.RemoveRange(photos);
        context.Users.Remove(user);

        await context.SaveChangesAsync(cancellationToken);
        await mediaContext.SaveChangesAsync(cancellationToken);
        logging.LogInformation($"Deleted user with id {user.Id}");

        return Result.Success(user.MapToCommandResponse());
    }
}
