using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Shared;
using SM.Domain.Users;

namespace SM.Application.Users.DeleteAccount;

internal class DeleteAccountCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory) 
    : ICommandHandler<DeleteAccountCommand, UserCommandResponse>
{
    public async Task<Result<UserCommandResponse>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var user = context.Users.FirstOrDefault(u => u.Id == request.Id);

        if (user is null)
            return Result.Failure<UserCommandResponse>(UserErrors.NotFound);

        context.Users.Remove(user);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(user.MapToCommandResponse());
    }
}
