using Microsoft.EntityFrameworkCore;
using SM.Application.Database;
using SM.Application.Users.DeleteAccount;
using SM.Domain.Users;

namespace SM.Application.UnitTests.Users.DeleteAccount;

public class DeleteAccountCommandHandlerTests
{
    [Fact]
    public async Task Handle_UserExists_ShouldDeleteUser()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("DeleteUserTest").Options;
        var context = new ApplicationDbContext(options);
        var user = User.Create("bkc_nr1", "Heinrich", "Lunge", "lunge@bkc.de");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var handler = new DeleteAccountCommandHandler(context);
        var command = new DeleteAccountCommand(user.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            context.Users.Any(u => u.Id == user.Id).Should().BeFalse();
        }
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ShouldReturnFailure()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("DeleteNonExistingUser").Options;
        var context = new ApplicationDbContext(options);
        var handler = new DeleteAccountCommandHandler(context);
        var command = new DeleteAccountCommand(Guid.NewGuid());

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NotFound);
        }
    }
}
