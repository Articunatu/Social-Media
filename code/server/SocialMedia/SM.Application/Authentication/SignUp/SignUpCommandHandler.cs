using MediatR;
using SM.Application.Database;
using SM.Domain.Shared;
using SM.Domain.Users;

namespace SM.Application.Authentication.SignUp;

internal class SignUpCommandHandler(ApplicationDbContext context, IJwtService jwtService)
    : IRequestHandler<SignUpCommand, Result>
{
    public async Task<Result> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        jwtService.GeneratePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = User.Create(request.Tag, request.FirstName, request.LastName, request.Email);
        user.SetLogin(passwordHash, passwordSalt);

        context.Users.Add(user);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(user);
    }
}


