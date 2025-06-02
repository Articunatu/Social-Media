using MediatR;
using SM.Domain.Shared;
using SM.Domain.Users;

namespace SM.Application.Authentication.SignUp;

public record SignUpCommand : IRequest<Result>, IFullName
{
    public string Tag { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}