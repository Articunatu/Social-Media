using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Authentication.ChangePassword;

internal class ChangePasswordCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory, IJwtService jwt) 
    : ICommandHandler<ChangePasswordCommand, string>
{
    public async Task<Result<string>> Handle(ChangePasswordCommand command, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        jwt.GeneratePasswordHash(command.OldPassword, out byte[] oldPasswordHash, out byte[] oldPasswordSalt);

        if (!jwt.VerifyPasswordHash(command.OldPassword, oldPasswordHash, oldPasswordSalt))
        {
            return Result.Failure<string>(new Error("Credentials invalid"), HttpStatusCode.BadRequest);
        }

        jwt.GeneratePasswordHash(command.NewPassword, out byte[] newPasswordHash, out byte[] newPasswordSalt);

        int updated = await context.Users
            .Where(u => u.Id == command.UserId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.PasswordHash, newPasswordHash)
                .SetProperty(u => u.PasswordSalt, newPasswordSalt),
                ct);

        if (updated <= 0)
        {
            return Result.Failure<string>(new Error("Updating password failed"), HttpStatusCode.BadRequest);
        }

        return Result.Success("PasswordChangeSuccess");
    }
}
