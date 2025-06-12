//using Microsoft.EntityFrameworkCore;
//using SM.Application.Database;
//using SM.Application.Users.Unfollow;
//using SM.Domain.Users;

//namespace SM.Application.UnitTests.Users.Unfollow;

//public class UnfollowCommandHandlerTests
//{
//    [Fact]
//    public async Task Handle_ShouldRemoveFollowRelation_WhenUsersExist()
//    {
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .UseInMemoryDatabase("UnfollowUserTest").Options;
//        var context = new ApplicationDbContext(options);
//        var follower = new User { Id = Guid.NewGuid() };
//        var following = new User { Id = Guid.NewGuid() };
//        follower.Following.Add(following);
//        following.Followers.Add(follower);
//        context.Users.AddRange(follower, following);
//        await context.SaveChangesAsync();
//        var handler = new UnfollowCommandHandler(context);
//        var command = new UnfollowCommand { FollowerId = follower.Id, FollowingId = following.Id };

//        var result = await handler.Handle(command, CancellationToken.None);

//        result.IsSuccess.Should().BeTrue();
//        follower.Following.Should().NotContain(following);
//        following.Followers.Should().NotContain(follower);
//    }

//    [Fact]
//    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
//    {
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .UseInMemoryDatabase("UnfollowFail").Options;
//        var context = new ApplicationDbContext(options);
//        var follower = new User { Id = Guid.NewGuid() };
//        context.Users.Add(follower);
//        await context.SaveChangesAsync();
//        var handler = new UnfollowCommandHandler(context);
//        var command = new UnfollowCommand { FollowerId = follower.Id, FollowingId = Guid.NewGuid() };

//        var result = await handler.Handle(command, CancellationToken.None);

//        result.IsSuccess.Should().BeFalse();
//        result.Error.Should().Be(UserErrors.NotFound);
//    }
//}
