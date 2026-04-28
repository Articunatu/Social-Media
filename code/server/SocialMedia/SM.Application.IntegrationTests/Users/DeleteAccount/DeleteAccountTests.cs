using Microsoft.EntityFrameworkCore;
using SM.Application.IntegrationTests.IntegrationAbstractions;
using SM.Application.Users.DeleteAccount;
using SM.Domain.Users;

namespace SM.Application.IntegrationTests.Users.DeleteAccount;

public class DeleteAccountTests(IntegrationTestFixture fixture) : BaseIntegrationTest(fixture)
{
    [Fact]
    public async Task Handle_UserExists_ShouldDeleteUser()
    {
        var user = User.Create("bkc_nr1", "Heinrich", "Lunge", "lunge@bkc.de");
        await Users.AddAsync(user);

        var result = await Sender.Send(new DeleteAccountCommand(user.Id));

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();

            var exists = await DbContext.Users.AnyAsync(u => u.Id == user.Id);
            exists.Should().BeFalse();
        }
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ShouldReturnFailure()
    {
        var result = await Sender.Send(new DeleteAccountCommand(Guid.NewGuid()));

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NotFound);
        }
    }
}
