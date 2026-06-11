using SM.Application.Authentication.ChangePassword;
using SM.Application.Authentication.Login;
using SM.Application.Authentication.SignUp;
using SM.Application.IntegrationTests.IntegrationAbstractions;
using System.Net;

namespace SM.Application.IntegrationTests.Authentication.ChangePassword;

public class ChangePasswordTests(IntegrationTestFixture fixture) : BaseIntegrationTest(fixture)
{
    [Fact]
    public async Task Handle_ShouldRejectWrongOldPasswordAndKeepExistingPassword()
    {
        const string tag = "password_owner";
        const string originalPassword = "OldPassword123!";

        var signUpResult = await Sender.Send(new SignUpCommand
        {
            Tag = tag,
            Email = "password.owner@example.com",
            FirstName = "Password",
            LastName = "Owner",
            Password = originalPassword
        });

        var result = await Sender.Send(new ChangePasswordCommand(
            signUpResult.Value!.Id,
            "WrongPassword123!",
            "NewPassword123!",
            "NewPassword123!"));

        var oldPasswordLoginResult = await Sender.Send(new LoginCommand(tag, originalPassword));
        var newPasswordLoginResult = await Sender.Send(new LoginCommand(tag, "NewPassword123!"));

        using (new AssertionScope())
        {
            signUpResult.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeTrue();
            result.Status.Should().Be(HttpStatusCode.BadRequest);
            result.Error.Header.Should().Be("Credentials invalid");
            oldPasswordLoginResult.IsSuccess.Should().BeTrue();
            newPasswordLoginResult.IsFailure.Should().BeTrue();
            newPasswordLoginResult.Status.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
