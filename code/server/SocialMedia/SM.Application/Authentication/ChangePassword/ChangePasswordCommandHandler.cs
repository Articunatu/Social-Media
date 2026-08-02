using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Behaviors;
using SM.Application.Database;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Authentication.ChangePassword;

internal class ChangePasswordCommandHandler(IDbContextFactory<IdentityDbContext> contextFactory, IJwtService jwt, ILoggingBehaviour logging) 
    : ICommandHandler<ChangePasswordCommand, string>
{
    public async Task<Result<string>> Handle(ChangePasswordCommand command, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var user = await context.Users
            .Where(u => u.Id == command.UserId)
            .Select(u => new { u.PasswordHash, u.PasswordSalt })
            .FirstOrDefaultAsync(ct);

        if (user is null)
            return Result.Failure<string>(new Error("User.NotFound"), HttpStatusCode.NotFound);

        if (!jwt.VerifyPasswordHash(command.OldPassword, user.PasswordHash, user.PasswordSalt))
            return Result.Failure<string>(new Error("Credentials invalid"), HttpStatusCode.BadRequest);

        jwt.GeneratePasswordHash(command.NewPassword, out byte[] newPasswordHash, out byte[] newPasswordSalt);

        int updated = await context.Users
            .Where(u => u.Id == command.UserId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.PasswordHash, newPasswordHash)
                .SetProperty(u => u.PasswordSalt, newPasswordSalt),
                ct);

        if (updated <= 0)
            return Result.Failure<string>(new Error("Updating password failed"), HttpStatusCode.BadRequest);

        logging.LogInformation($"Password updated for user with id {command.UserId}");
        return Result.Success("PasswordChangeSuccess");
    }
}
