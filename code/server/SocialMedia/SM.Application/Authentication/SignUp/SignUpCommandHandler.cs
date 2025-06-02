using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Application.Database;
using SM.Domain.Shared;

namespace SM.Application.Authentication.SignUp;

internal class SignUpCommandHandler(ApplicationDbContext context, IJwtService jwtService)
    : IRequestHandler<SignUpCommand, Result>
{
    public async Task<Result> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        jwtService.GeneratePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        int updated = await context.Users
            .Where(u => u.Tag == request.Tag)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.PasswordHash, passwordHash)
                .SetProperty(u => u.PasswordSalt, passwordSalt),
                cancellationToken);

        return updated == 1
            ? Result.Success()
            : Result.Failure(new Error("User not found or update failed."));
    }
}


