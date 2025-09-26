using SM.Application.Abstractions;
using SM.Application.Authentication.SignUp.Models;
using static SM.Domain.Users.User;

namespace SM.Application.Authentication.SignUp;

public record SignUpCommand : ICommand<SignUpResponse>, IUser
{
    public string Tag { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}