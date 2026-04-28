using FluentValidation;
using SM.Domain.Users.ValueObjects;

namespace SM.Application.Authentication.SignUp.Validators;

internal class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.Tag)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(Tag.MaxLength)
            .Matches(Tag.Pattern);

        RuleFor(x => x.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(FirstName.MinLength)
            .MaximumLength(FirstName.MaxLength)
            .Matches(FirstName.Pattern);

        RuleFor(x => x.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(FirstName.MinLength)
            .MaximumLength(LastName.MaxLength)
            .Matches(FirstName.Pattern);

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(Email.MaxLength)
            .EmailAddress()
            .Matches(Email.Pattern);

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(Password.MinLength)
            .MaximumLength(Password.MaxLength)
            .Matches(Password.Pattern);
    }
}
