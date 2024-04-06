using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Users.Commands.AddUserCommand
{
    public sealed record AddUserCommand(
        string Tag,
        string Email,
        string FirstName,
        string LastName,
        string Password) : ICommand<Guid>;
}