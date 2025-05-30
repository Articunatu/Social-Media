using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using SM.Application.Database;
using SM.Application.Profiles.GetProfilePosts;
using SM.Domain.Users;

namespace SM.Application.UnitTests;

public class GetUserByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnCorrectUser_WhenUserExists()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        using var context = new ApplicationDbContext(options);
        var user = User.Create("greatsteiner", "Hans", "Grimner", "grimner@gmail.com");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var handler = new GetProfilePostsQueryHandler(context);
        var query = new GetProfilePostsQuery(user.Id, default!);

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Value.UserProfile.Id.Should().Be(user.Id);
            result.Value.UserProfile.Should().Be("Alice");
        }
    }

    //[Fact]
    //public async Task Handle_ShouldReturnNull_WhenUserDoesNotExist()
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<AppDbContext>()
    //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    //        .Options;

    //    using var context = new AppDbContext(options);

    //    var handler = new GetUserByIdQueryHandler(context);
    //    var query = new GetUserByIdQuery(Guid.NewGuid());

    //    // Act
    //    var result = await handler.Handle(query, CancellationToken.None);

    //    // Assert
    //    result.Should().BeNull();
    //}
}