using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Users.LogInUser
{
    public sealed record LogInUserCommand(string Email, string Password)
    : ICommand<object>;
}
