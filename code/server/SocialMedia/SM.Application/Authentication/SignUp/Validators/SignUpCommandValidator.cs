using FluentValidation;
using SM.Domain.Users.ValueObjects;

namespace SM.Application.Authentication.SignUp.Validators;

internal class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.Tag)
            .NotEmpty()
            .MaximumLength(Tag.MaxLength)
            .Matches(Tag.Pattern);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(FirstName.MinLength)
            .MaximumLength(FirstName.MaxLength)
            .Matches(FirstName.Pattern);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(FirstName.MinLength)
            .MaximumLength(LastName.MaxLength)
            .Matches(FirstName.Pattern);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(Email.MaxLength)
            .EmailAddress()
            .Matches(Email.Pattern);

        RuleFor(x => x.Password)
            .MinimumLength(Password.MinLength)
            .MaximumLength(Password.MaxLength)
            .Matches(Password.Pattern);
    }
}
