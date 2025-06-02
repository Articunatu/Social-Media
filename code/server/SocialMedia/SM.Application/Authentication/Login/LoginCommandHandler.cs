using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SM.Application.Database;

namespace SM.Application.Authentication.Login;

internal class LoginCommandHandler(IJwtService jwtService, IConfiguration config, ApplicationDbContext context) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userAuth = await context.Users
                .Where(u => u.Tag == request.Tag)
                .Select(u => new
                {
                    u.Id,
                    u.PasswordHash,
                    u.PasswordSalt
                })
                .FirstOrDefaultAsync(cancellationToken);
        if (userAuth is null || !jwtService.VerifyPasswordHash(request.Password, userAuth.PasswordHash, userAuth.PasswordSalt))
            throw new UnauthorizedAccessException("Invalid credentials");

        string accessToken = jwtService.CreateToken(userAuth.Id.ToString(), config["AppSettings:Token"]!);
        var refreshToken = jwtService.GenerateRefreshToken();
        refreshToken.UserId = userAuth.Id;

        await context.Tokens.AddAsync(refreshToken, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new LoginResponse(accessToken, refreshToken);
    }
}

