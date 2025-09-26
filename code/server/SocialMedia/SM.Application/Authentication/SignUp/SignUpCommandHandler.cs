using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Authentication.SignUp.Extensions;
using SM.Application.Authentication.SignUp.Models;
using SM.Application.Database;
using SM.Domain.Shared;
using SM.Domain.Users;
using System.Net;

namespace SM.Application.Authentication.SignUp;

internal class SignUpCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory, IJwtService jwtService)
    : ICommandHandler<SignUpCommand, SignUpResponse>
{
    public async Task<Result<SignUpResponse>> Handle(SignUpCommand dto, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var existingUser = await context.Users
            .Where(u => u.Email == dto.Email || u.Tag == dto.Tag)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser is not null)
        {
            return Result.Failure<SignUpResponse>(
                new Error("A user with this email or tag already exists"), HttpStatusCode.Conflict);
        }

        jwtService.GeneratePasswordHash(dto.Password, out var passwordHash, out var passwordSalt);

        var user = User.Create(dto);
        user.SetLogin(passwordHash, passwordSalt);

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        var response = user.MapToSignUpResponse();

        return Result.Success(response);
    }
}
