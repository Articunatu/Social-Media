using FluentValidation;

namespace SM.Application.Authentication.SignUp;

internal class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.Tag)
            .NotEmpty()
            .MaximumLength(25);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(25);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(35);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(50)
            .EmailAddress();

        RuleFor(x => x.Password)
            .MinimumLength(5);
    }
}
