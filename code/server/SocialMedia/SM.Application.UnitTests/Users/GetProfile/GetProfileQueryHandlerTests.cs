//using Microsoft.EntityFrameworkCore;
//using SM.Application.Database;
//using SM.Application.Users.GetProfile;
//using SM.Domain.Photos;
//using SM.Domain.Users;

//namespace SM.Application.UnitTests.Users.GetProfile;

//public class GetProfileQueryHandlerTests
//{
//    [Fact]
//    public async Task Handle_ShouldReturnProfileDetails_WhenUserExists()
//    {
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .UseInMemoryDatabase("ProfileQuery").Options;
//        var context = new ApplicationDbContext(options);

//        var user = new User
//        {
//            Id = Guid.NewGuid(),
//            Followers = new List<User>(),
//            Following = new List<User>(),
//            AuthoredMessages = new List<Message>
//    {
//        new Message { Content = "About me" }
//    },
//            Photos = new List<Photo>
//    {
//        new Photo { Type = PhotoType.Background }
//    }
//        };

//        context.Users.Add(user);
//        await context.SaveChangesAsync();

//        var handler = new GetProfileQueryHandler(context);
//        var query = new GetProfileQuery { Id = user.Id };

//        var result = await handler.Handle(query, CancellationToken.None);

//        result.IsSuccess.Should().BeTrue();
//        result.Value.Profile.Should().NotBeNull();
//        result.Value.AboutMe.Should().Be("About me");
//        result.Value.BackgroundPhoto.Should().NotBeNull();
//    }

//    [Fact]
//    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
//    {
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .UseInMemoryDatabase("MissingProfile").Options;
//        var context = new ApplicationDbContext(options);

//        var handler = new GetProfileQueryHandler(context);
//        var query = new GetProfileQuery { Id = Guid.NewGuid() };

//        var result = await handler.Handle(query, CancellationToken.None);

//        result.IsSuccess.Should().BeFalse();
//        result.Error.Should().Be(UserErrors.NotFound);
//    }

//}
