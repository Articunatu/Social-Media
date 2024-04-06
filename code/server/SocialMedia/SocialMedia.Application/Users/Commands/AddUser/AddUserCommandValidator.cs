using FluentValidation;
using SocialMedia.Domain.Users.ValueObjects;

namespace SocialMedia.Application.Users.Commands.AddUserCommand
{
    internal class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            RuleFor(x => x.Tag).NotEmpty()
                .MinimumLength(Tag.MinLength).MaximumLength(Tag.MaxLength)
                .Matches(Tag.Pattern);
            RuleFor(x => x.Email).NotEmpty().Matches(Email.Pattern);
            RuleFor(x => x.FirstName).NotEmpty()
                .MinimumLength(Name.MinLength).MaximumLength(Name.MaxLength)
                .Matches(Name.Pattern);
            RuleFor(x => x.LastName).NotEmpty()
                .MinimumLength(Name.MinLength).MaximumLength(Name.MaxLength)
                .Matches(Name.Pattern);
        }
    }
}
