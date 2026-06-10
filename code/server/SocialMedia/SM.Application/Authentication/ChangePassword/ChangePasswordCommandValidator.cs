using FluentValidation;
using SM.Domain.Users.ValueObjects;

namespace SM.Application.Authentication.ChangePassword;

internal class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(Password.MinLength)
            .MaximumLength(Password.MaxLength)
            .Matches(Password.Pattern);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword);
    }
}
