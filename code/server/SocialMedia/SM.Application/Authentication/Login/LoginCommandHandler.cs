using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Shared;
using SM.Domain.Users;
using System.Net;

namespace SM.Application.Authentication.Login;

internal class LoginCommandHandler(IJwtService jwtService, IConfiguration config, IDbContextFactory<ApplicationDbContext> contextFactory) 
    : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var userAuth = await context.Users
                .Where(u => u.Tag == request.Tag)
                .Select(u => new
                {
                    u.Id,
                    u.PasswordHash,
                    u.PasswordSalt
                })
                .FirstOrDefaultAsync(cancellationToken);

        if (userAuth is null)
            return Result.Failure<LoginResponse>(new Error(UserErrors.NotFound), HttpStatusCode.NotFound);

        if (!jwtService.VerifyPasswordHash(request.Password, userAuth.PasswordHash, userAuth.PasswordSalt))
        {
            return Result.Failure<LoginResponse>(new Error("Credentials invalid"), HttpStatusCode.BadRequest);
        }

        string accessToken = jwtService.CreateToken(userAuth.Id.ToString(), config["AppSettings:Token"]!);
        var refreshToken = jwtService.GenerateRefreshToken();
        if (refreshToken is null)
        {
            return Result.Failure<LoginResponse>(new Error("Token could not be refreshed"), HttpStatusCode.Unauthorized);
        }

        refreshToken.UserId = userAuth.Id;

        await context.Tokens.AddAsync(refreshToken, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var loginResponse = new LoginResponse(accessToken, refreshToken);
        return Result.Success(loginResponse);
    }
}

