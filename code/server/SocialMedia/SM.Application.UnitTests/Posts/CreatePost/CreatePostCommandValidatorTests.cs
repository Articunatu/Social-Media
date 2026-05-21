using FluentValidation.TestHelper;
using SM.Application.Posts.CreatePost;
using SM.Domain.Messages.ValueObjects;

namespace SM.Application.UnitTests.Posts.CreatePost;

public class CreatePostCommandValidatorTests
{
    private readonly CreatePostCommandValidator _validator = new();

    [Fact]
    public void ValidCommand_ShouldNotHaveValidationErrors()
    {
        var command = new CreatePostCommand("Hello world", Guid.NewGuid());

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void MissingAuthor_ShouldHaveValidationError()
    {
        var command = new CreatePostCommand("Hello world", Guid.Empty);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.AuthorId);
    }

    [Fact]
    public void EmptyContent_ShouldHaveValidationError()
    {
        var command = new CreatePostCommand(string.Empty, Guid.NewGuid());

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.Content);
    }

    [Fact]
    public void TooLongContent_ShouldHaveValidationError()
    {
        var longContent = new string('a', Content.MaxLength + 1);
        var command = new CreatePostCommand(longContent, Guid.NewGuid());

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.Content);
    }
}
