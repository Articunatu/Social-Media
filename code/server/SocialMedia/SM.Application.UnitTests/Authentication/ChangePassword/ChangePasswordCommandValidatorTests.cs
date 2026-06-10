using FluentValidation.TestHelper;
using SM.Application.Authentication.ChangePassword;

namespace SM.Application.UnitTests.Authentication.ChangePassword;

public class ChangePasswordCommandValidatorTests
{
    private readonly ChangePasswordCommandValidator _validator = new();

    [Fact]
    public void ValidCommand_ShouldNotHaveValidationErrors()
    {
        var command = new ChangePasswordCommand(Guid.NewGuid(), "OldPassword123!", "NewPassword123!", "NewPassword123!");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void InvalidPassword_ShouldHaveValidationErrors()
    {
        var command = new ChangePasswordCommand(Guid.NewGuid(), "OldPassword123!", "NewPassword123", "NewPassword123");

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.NewPassword);
    }

    [Fact]
    public void MismatchedConfirmPassword_ShouldHaveValidationErrors()
    {
        var command = new ChangePasswordCommand(Guid.NewGuid(), "OldPassword123!", "NewPassword123!", "OtherPassword123!");

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.ConfirmPassword);
    }
}
