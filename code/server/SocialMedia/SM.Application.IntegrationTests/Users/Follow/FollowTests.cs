using Microsoft.EntityFrameworkCore;
using SM.Application.IntegrationTests.IntegrationAbstractions;
using SM.Application.Users.Follow;
using SM.Domain.Users;

namespace SM.Application.IntegrationTests.Users.Follow;

public class FollowTests(IntegrationTestFixture fixture) : BaseIntegrationTest(fixture)
{
    [Fact]
    public async Task Handle_ShouldAddFollowRelation_WhenUsersExist()
    {
        var follower = User.Create(new UserDto("bkc_nr1", "Heinrich", "Lunge", "lunge@bkc.de"));
        var following = User.Create(new UserDto("rose_duelist", "Utena", "Tenjou", "revolutionary@shoujo.jp"));

        await Users.AddAsync(follower, following);

        var result = await Sender.Send(new FollowCommand(follower.Id, following.Id));

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();

            var followerFromDb = await DbContext.Users.Include(u => u.Following).FirstOrDefaultAsync(u => u.Id == follower.Id);
            var followingFromDb = await DbContext.Users.Include(u => u.Followers).FirstOrDefaultAsync(u => u.Id == following.Id);

            followerFromDb!.Following.Should().Contain(f => f.Id == following.Id);
            followingFromDb!.Followers.Should().Contain(f => f.Id == follower.Id);
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAnyUserNotFound()
    {
        var follower = User.Create(new UserDto("solid_warrior", "Reinar", "Braunn", "reinar_braunn@atk.ttn"));
        await Users.AddAsync(follower);

        var result = await Sender.Send(new FollowCommand(follower.Id, Guid.NewGuid()));

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NotFound);
        }
    }
}
