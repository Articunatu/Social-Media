using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Users.Commands.AddUser
{
    public sealed record AddUserCommand(
        string Tag,
        string Email,
        string FirstName,
        string LastName) : ICommand;
}