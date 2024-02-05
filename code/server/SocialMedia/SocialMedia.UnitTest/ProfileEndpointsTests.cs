using System.Net;
using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SocialMedia.Application.Users.Queries.GetUserById;
using Xunit;
using MediatR;
using System.Net.Http.Json;
using Microsoft.AspNetCore.TestHost;
using SocialMedia.Application.Users.Queries.Application.Users.Queries.GetUserById;

public class ProfileEndpointTests
{
    //[Fact]
    //public async Task Can_Get_Profile_Info()
    //{
    //    // Arrange
    //    var mockMediator = new Mock<IMediator>();

    //    // Replace 'expectedUser' with a user object that you expect to be returned
    //    var expectedUser = new UserResponse { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

    //    mockMediator.Setup(x => x.Send(It.IsAny<GetUserByIdQuery>(), default))
    //        .ReturnsAsync(expectedUser);


    //    var builder = new WebHostBuilder()
    //        .ConfigureServices(services =>
    //        {
    //            services.AddCarter();
    //            services.AddSingleton(mockMediator.Object);
    //        })
    //        .Configure(app =>
    //        {
    //            app.UseRouting();
    //            app.UseEndpoints(endpoints =>
    //            {
    //                endpoints.MapCarter();
    //            });
    //        });

    //    using (var server = new TestServer(builder))
    //    {
    //        var client = server.CreateClient();

    //        // Act
    //        var response = await client.GetAsync("/api/profile/123456"); // Replace 123456 with a valid Guid

    //        // Assert
    //        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    //        // Additional assertions if needed
    //    }
    //}

    [Fact]
    public async Task Can_Add_New_User()
    {
        // Arrange
        var mockMediator = new Mock<IMediator>();

        var builder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddRouting(); // Add the required routing services
                services.AddCarter();
                services.AddSingleton(mockMediator.Object);
            })
            .Configure(app =>
            {
                app.UseRouting(); // Use the routing services
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapCarter();
                });
            });

        using (var server = new TestServer(builder))
        {
            var client = server.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/api/profile/adduser", new
            {
                Tag = "TestUser",
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe"
            });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Additional assertions if needed
        }
    }

}
