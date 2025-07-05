using Microsoft.EntityFrameworkCore;
using SM.Application.Database;
using SM.Application.UnitTests.Helpers;
using SM.Application.Users.Unfollow;
using SM.Domain.Users;

namespace SM.Application.UnitTests.Users.Unfollow;

public class UnfollowCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldRemoveFollowRelation_WhenUsersExist()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("UnfollowUserTest").Options;
        var context = new ApplicationDbContext(options);
        var follower = User.Create("bkc_nr1", "Heinrich", "Lunge", "lunge@bkc.de");
        var following = User.Create("rose_duelist", "Utena", "Tenjou", "revolutionary@shoujo.jp");
        follower.Following.Add(following);
        following.Followers.Add(follower);
        context.Users.AddRange(follower, following);
        await context.SaveChangesAsync();
        var factory = context.CreateSubstituteFactory();
        var handler = new UnfollowCommandHandler(factory);
        var command = new UnfollowCommand(follower.Id, following.Id);

        var unfollow = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            unfollow.IsSuccess.Should().BeTrue();
            follower.Following.Should().NotContain(following);
            following.Followers.Should().NotContain(follower);
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("UnfollowFail").Options;
        var context = new ApplicationDbContext(options);
        var follower = User.Create("solid_warrior", "Reinar", "Braunn", "reinar_braunn@atk.ttn");
        context.Users.Add(follower);
        await context.SaveChangesAsync();
        var factory = context.CreateSubstituteFactory();
        var handler = new UnfollowCommandHandler(factory);
        var command = new UnfollowCommand(follower.Id, Guid.NewGuid());

        var unfollow = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            unfollow.IsSuccess.Should().BeFalse();
            unfollow.Error.Should().Be(UserErrors.NotFound);
        }
    }
}
