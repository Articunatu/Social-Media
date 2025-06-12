using Microsoft.EntityFrameworkCore;
using SM.Application.Database;
using SM.Application.Users.Follow;
using SM.Domain.Users;

namespace SM.Application.UnitTests.Users.Follow;

public class FollowCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddFollowRelation_WhenUsersExist()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("FollowUserTest").Options;
        var context = new ApplicationDbContext(options);
        var follower = User.Create("bkc_nr1", "Heinrich", "Lunge", "lunge@bkc.de");
        var following = User.Create("rose_duelist", "Utena", "Tenjou", "revolutionary@shoujo.jp");
        context.Users.AddRange(follower, following);
        await context.SaveChangesAsync();
        var handler = new FollowCommandHandler(context);
        var command = new FollowCommand(follower.Id, following.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            follower.Following.Should().Contain(following);
            following.Followers.Should().Contain(follower);
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAnyUserNotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("FollowFail").Options;
        var context = new ApplicationDbContext(options);
        var follower = User.Create("solid_warrior", "Reinar", "Braunn", "reinar_braunn@atk.ttn");
        context.Users.Add(follower);
        await context.SaveChangesAsync();
        var handler = new FollowCommandHandler(context);
        var command = new FollowCommand(follower.Id, Guid.NewGuid());

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NotFound);
        }
    }

}
