using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SM.Application.Abstractions;
using SM.Application.Authentication.Login.Models;
using SM.Application.Behaviors;
using SM.Application.Database;
using SM.Domain.Authentication;
using SM.Domain.Shared;
using SM.Domain.Users;
using SM.Domain.Users.ValueObjects;
using System.Net;
using System.Text.RegularExpressions;

namespace SM.Application.Authentication.Login;

internal class LoginCommandHandler(IJwtService jwtService, IConfiguration config, IDbContextFactory<ApplicationDbContext> contextFactory, ILoggingBehaviour logging)
    : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var userAuth = await GetUserAuthAsync(context, request.Tag, ct);
        if (userAuth is null)
            return Failure(UserErrors.NotFound, HttpStatusCode.NotFound);

        if (!jwtService.VerifyPasswordHash(request.Password, userAuth.PasswordHash, userAuth.PasswordSalt))
        {
            logging.LogWarning($"Failed login attempt for user with tag {request.Tag}");
        }

        var accessToken = jwtService.CreateToken(userAuth.Id.ToString(), userAuth.Tag, config["AppSettings:Token"]!);
        var refreshToken = jwtService.GenerateRefreshToken();

        if (refreshToken is null)
            return Failure("TokenFailedRefresh", HttpStatusCode.Unauthorized);

        refreshToken.UserId = userAuth.Id;
        await UpsertRefreshTokenAsync(context, refreshToken, ct);
        logging.LogInformation($"User with tag {request.Tag} logged in successfully");

        var loginResponse = new LoginResponse(accessToken, refreshToken);
        return Result.Success(loginResponse);
    }

    private static async Task<UserAuthDto?> GetUserAuthAsync(ApplicationDbContext context, string tag, CancellationToken ct)
    {
        bool isTagEmail = Regex.IsMatch(tag, Email.Pattern);

        return await context.Users
            .Where(isTagEmail 
                ? u => u.Email == tag 
                : u => u.Tag == tag)
            .Select(u => new UserAuthDto(u.Id, u.Tag, u.PasswordHash, u.PasswordSalt))
            .FirstOrDefaultAsync(ct);
    }

    private static async Task UpsertRefreshTokenAsync(ApplicationDbContext context, Token refreshToken, CancellationToken ct)
    {
        var existingToken = await context.Tokens.FirstOrDefaultAsync(t => t.UserId == refreshToken.UserId, ct);

        if (existingToken is null)
        {
            await context.Tokens.AddAsync(refreshToken, ct);
            await context.SaveChangesAsync(ct);
            return;
        }
        existingToken.Created = refreshToken.Created;
        existingToken.Expires = refreshToken.Expires;

        await context.SaveChangesAsync(ct);
    }

    private static Result<LoginResponse> Failure(string message, HttpStatusCode statusCode) =>
        Result.Failure<LoginResponse>(new Error(message), statusCode);

    private static Result<LoginResponse> Failure(Error error, HttpStatusCode statusCode) =>
        Result.Failure<LoginResponse>(error, statusCode);
}
