using Microsoft.EntityFrameworkCore;
using NSubstitute;
using SM.Application.Database;
using SM.Application.UnitTests.Helpers;
using SM.Application.Users.DeleteAccount;
using SM.Domain.Users;

namespace SM.Application.UnitTests.Users.DeleteAccount;

public class DeleteAccountCommandHandlerTests
{
    [Fact]
    public async Task Handle_UserExists_ShouldDeleteUser()
    {
        var dbName = "DeleteUserTest";
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        await using (var seedContext = new ApplicationDbContext(options))
        {
            var user = User.Create("bkc_nr1", "Heinrich", "Lunge", "lunge@bkc.de");
            seedContext.Users.Add(user);
            await seedContext.SaveChangesAsync();
        }
        var factory = Substitute.For<IDbContextFactory<ApplicationDbContext>>();
        factory.CreateDbContextAsync(Arg.Any<CancellationToken>())
               .Returns(ci => new ApplicationDbContext(options));
        var handler = new DeleteAccountCommandHandler(factory);
        Guid userId;
        await using (var checkContext = new ApplicationDbContext(options))
        {
            userId = checkContext.Users.Select(u => u.Id).First();
        }
        var command = new DeleteAccountCommand(userId);

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();

            await using var assertContext = new ApplicationDbContext(options);
            assertContext.Users.Any(u => u.Id == userId).Should().BeFalse();
        }
    }


    [Fact]
    public async Task Handle_UserDoesNotExist_ShouldReturnFailure()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "DeleteNonExistingUser")
            .Options;
        var context = new ApplicationDbContext(options);
        var factory = context.CreateSubstituteFactory();
        var handler = new DeleteAccountCommandHandler(factory);
        var command = new DeleteAccountCommand(Guid.NewGuid());

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NotFound);
        }
    }
}
