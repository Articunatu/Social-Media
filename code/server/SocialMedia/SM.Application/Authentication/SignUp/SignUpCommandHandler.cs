using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Shared;
using SM.Domain.Users;

namespace SM.Application.Authentication.SignUp;

internal class SignUpCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory, IJwtService jwtService)
    : ICommandHandler<SignUpCommand, SignUpResponse>
{
    public async Task<Result<SignUpResponse>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        jwtService.GeneratePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = User.Create(request.Tag, request.FirstName, request.LastName, request.Email);
        user.SetLogin(passwordHash, passwordSalt);

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        var response = user.MapToSignUpResponse();

        return Result.Success(response);
    }
}
