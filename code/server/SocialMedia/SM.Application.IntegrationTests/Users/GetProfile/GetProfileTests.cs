using SM.Application.IntegrationTests.IntegrationAbstractions;
using SM.Application.Users.GetProfile;
using SM.Domain.Users;

namespace SM.Application.IntegrationTests.Users.GetProfile;

public class GetProfileTests(IntegrationTestFixture fixture) : BaseIntegrationTest(fixture)
{
    [Fact]
    public async Task Handle_ShouldReturnProfileDetails_WhenUserExists()
    {
        var userId = await Users.CreateProfileUserAsync(
            aboutMe: "About me",
            withBackgroundPhoto: true);

        var result = await Sender.Send(new GetProfileQuery(userId, Guid.Empty));

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value!.Profile.Should().NotBeNull();
            result.Value.AboutMe.Should().Be("About me");
            result.Value.BackgroundPhoto.Should().NotBeNull();
            result.Value.IsFollowedByCurrentUser.Should().BeFalse();
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
    {
        var result = await Sender.Send(new GetProfileQuery(Guid.NewGuid(), Guid.Empty));

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.NotFound);
    }
}
