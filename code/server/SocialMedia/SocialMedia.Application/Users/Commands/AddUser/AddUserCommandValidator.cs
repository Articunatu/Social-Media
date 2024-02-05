using FluentValidation;

namespace SocialMedia.Application.Users.Commands.AddUser
{
    internal class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            RuleFor(x => x.Tag).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty().MinimumLength(20);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(20);
        }
    }
}
