using Microsoft.EntityFrameworkCore;
using SM.Application.Authentication.SignUp;
using SM.Application.IntegrationTests.IntegrationAbstractions;
using SM.Domain.Users;

namespace SM.Application.IntegrationTests.Authentication.SignUp;

public class SignUpTests(IntegrationTestFixture fixture) : BaseIntegrationTest(fixture)
{
    [Fact]
    public async Task Handle_NewUser_ShouldCreateUserAndReturnResponse()
    {
        const string tag = "new_user_tag";
        const string email = "new.user@example.com";
        const string firstName = "New";
        const string lastName = "User";
        const string password = "Password123!";

        var result = await Sender.Send(new SignUpCommand
        {
            Tag = tag,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Password = password
        });

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            var response = result.Value!;
            response.Tag.Should().Be(tag);
            response.Email.Should().Be(email);
            response.FullName.Should().Be($"{firstName} {lastName}");

            var exists = await IdentityDbContext.Users.AnyAsync(u => u.Id == response.Id && u.Email == email && u.Tag == tag);
            exists.Should().BeTrue();

            var user = await IdentityDbContext.Users.FirstAsync(u => u.Id == response.Id);
            user.PasswordHash.Should().NotBeNull();
            user.PasswordHash.Length.Should().BeGreaterThan(0);
        }
    }

    [Fact]
    public async Task Handle_DuplicateEmailOrTag_ShouldReturnConflict()
    {
        var existingUser = User.Create(new UserDto("existing_tag", "Ex", "Ist", "existing@example.com"));
        await Users.AddAsync(existingUser);

        var result = await Sender.Send(new SignUpCommand
        {
            Tag = existingUser.Tag,
            Email = "new.email@example.com",
            FirstName = "Test",
            LastName = "User",
            Password = "Password123!"
        });

        using (new AssertionScope())
        {
            result.IsFailure.Should().BeTrue();
            result.Status.Should().Be(System.Net.HttpStatusCode.Conflict);
            result.Error.Header.Should().Be("A user with this email or tag already exists");
        }

        var result2 = await Sender.Send(new SignUpCommand
        {
            Tag = "another_tag",
            Email = existingUser.Email,
            FirstName = "Test",
            LastName = "User",
            Password = "Password123!"
        });

        using (new AssertionScope())
        {
            result2.IsFailure.Should().BeTrue();
            result2.Status.Should().Be(System.Net.HttpStatusCode.Conflict);
            result2.Error.Header.Should().Be("A user with this email or tag already exists");
        }
    }
}
