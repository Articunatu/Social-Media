using NSubstitute;
using Shouldly;
using SocialMedia.Application.Abstractions;
using SocialMedia.Application.Profiles.GetProfilePosts;
using SocialMedia.Application.Shared.Models;
using SocialMedia.Domain.Messages;

namespace SocialMedia.UnitTest.Application.UnitTests;

public class GetProfilePostsQueryHandlerTests
{
    private readonly IProfileRepository _profileRepository;
    private readonly GetProfilePostsQueryHandler _handler;

    public GetProfilePostsQueryHandlerTests()
    {
        _profileRepository = Substitute.For<IProfileRepository>();
        _handler = new GetProfilePostsQueryHandler(_profileRepository);
    }

    [Fact]
    public async Task Handle_PostsExist_Failure()
    {
        // Arrange
        var query = new GetProfilePostsQuery(Guid.NewGuid(), new PageFilter());

        var emptyResponse = new ProfileFeedResponse(
            new ProfileInfo(Guid.NewGuid(), "", "", null),
            new PagedFeed<ProfilePostDto>
            {
                Values = []
            });

        _profileRepository
            .GetPagedProfilePosts(query.UserId, query.Filter)
            .Returns(Task.FromResult(emptyResponse));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Header.ShouldBe("This user hasn't posted anything");
    }

    //[Fact]
    //public async Task Handle_ShouldReturnSuccess_WhenPostsExist()
    //{
    //    // Arrange
    //    var query = new GetProfilePostsQuery(Guid.NewGuid(), new PageFilter());

    //    var post = new Post { Content = "Pokemon"};
    //    var feedResponse = new ProfileFeedResponse
    //    {
    //        ProfileFeed = new PagedFeed<ProfilePostDto>
    //        {
    //            Values = new List<ProfilePostDto> { post }
    //        }
    //    };

    //    _profileRepository
    //        .GetPagedProfilePosts(query.UserId, query.Filter)
    //        .Returns(Task.FromResult(feedResponse));

    //    // Act
    //    var result = await _handler.Handle(query, CancellationToken.None);

    //    // Assert
    //    result.IsSuccess.ShouldBeTrue();
    //    result.Value.ShouldBe(feedResponse);
    //}
}

