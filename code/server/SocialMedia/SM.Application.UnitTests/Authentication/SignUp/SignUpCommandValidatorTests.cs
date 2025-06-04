using FluentAssertions;
using FluentValidation.TestHelper;
using SM.Application.Authentication.SignUp;

namespace SM.Application.UnitTests.Authentication.SignUp;

public class SignUpCommandValidatorTests
{
    private readonly SignUpCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("invalid!tag")]
    [InlineData("this_tag_is_way_too_long_to_be_valid")]
    public void Tag_VariousLengths_Error(string tag)
    {
        var command = new SignUpCommand { Tag = tag };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Tag);
    }

    [Fact]
    public void Tag_ValidFormat_Success()
    {
        var command = new SignUpCommand { Tag = "valid_tag123" };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Tag);
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    [InlineData("ThisNameIsWayTooLongToBeValidEvenByLiberalStandards")]
    [InlineData("123Invalid")]
    public void FirstName_NumbersAndInvalidLengths_Error(string firstName)
    {
        var command = new SignUpCommand { FirstName = firstName };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.FirstName);
    }

    [Fact]
    public void FirstName_Characters_Success()
    {
        var command = new SignUpCommand { FirstName = "Elodie" };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.FirstName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("X")]
    [InlineData("AnotherVeryLongLastNameThatIsTooBigToBeValid")]
    [InlineData("$$$")]
    public void LastName_SpecialCharsAndLengths_Error(string lastName)
    {
        var command = new SignUpCommand { LastName = lastName };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.LastName);
    }

    [Fact]
    public void LastName_ApostrophName_Success()
    {
        var command = new SignUpCommand { LastName = "O'Connor" };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.LastName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("plainaddress")]
    [InlineData("missing@dot")]
    public void Email_MissingAtsAndDots_Error(string email)
    {
        var command = new SignUpCommand { Email = email };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Email_TooLong_Error()
    {
        var longEmail = "toolong@" + new string('a', 90) + ".com";
        var command = new SignUpCommand { Email = longEmail };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Email);
    }


    [Fact]
    public void Email_ValidFormatWithPlusSign_Success()
    {
        var command = new SignUpCommand { Email = "test.user+test@example.com" };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }

    [Theory]
    [InlineData("Short1!")]
    [InlineData("nouppercase1!")]
    [InlineData("NOLOWERCASE1!")]
    [InlineData("NoNumber!")]
    [InlineData("NoSpecial1")]
    public void Password_LackingNumbersSpecialCharsAndCases_Error(string password)
    {
        var command = new SignUpCommand { Password = password };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public void Password_ValidFormatIncludingPlusSign_Success()
    {
        var command = new SignUpCommand { Password = "Strong1+Password" };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Password);
    }


    [Fact]
    public void AllUserProperties_ValidInputs_Success()
    {
        var command = new SignUpCommand
        {
            Tag = "valid_tag",
            FirstName = "Alice",
            LastName = "Smith",
            Email = "alice.smith@example.com",
            Password = "Valid1+Pass"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}
