using Application.Abstraction;

namespace Application.Users.Commands.AddUserCommand
{
    public sealed record AddUserCommand(
        string Tag,
        string Email,
        string FirstName,
        string LastName) : ICommand;
}
