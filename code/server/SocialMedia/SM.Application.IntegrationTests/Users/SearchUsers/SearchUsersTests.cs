using SM.Application.IntegrationTests.IntegrationAbstractions;
using SM.Application.Shared.Models;
using SM.Application.Users.SearchUsers;
using SM.Domain.Users;

namespace SM.Application.IntegrationTests.Users.SearchUsers;

public class SearchUsersTests(IntegrationTestFixture fixture) : BaseIntegrationTest(fixture)
{
    [Fact]
    public async Task Handle_SearchTextEin_ReturnsNamesAndTagsContainingIt()
    {
        await Users.AddAsync(
            User.Create(new UserDto("bkc_nr1", "Heinrich", "Lunge", "lunge@bkc.de")),
            User.Create(new UserDto("solid_warrior", "Reinar", "Braunn", "reinar_braunn@atk.ttn")),
            User.Create(new UserDto("rose_duelist", "Utena", "Tenjou", "revolutionary@shoujo.jp")));

        var result = await Sender.Send(
            new SearchUserQuery(new PageFilter { SearchText = "ein" }));

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Contain(x => x.FullName.Contains("ein", StringComparison.OrdinalIgnoreCase));
            result.Value.Should().NotContain(x => x.FullName == "Utena");
        }
    }
}