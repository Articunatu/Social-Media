using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Users.AddUser
{
    public sealed record AddUserCommand(
        string Tag,
        string Email,
        string FirstName,
        string LastName,
        string Password) : ICommand<Guid>;
}