using Domain.ValueObjects.User;
using FluentValidation;

namespace Application.Users.Commands.AddUserCommand
{
    internal class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            RuleFor(x => x.Tag).NotEmpty();

            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(FirstName.MaxLength);

        }
    }
}
