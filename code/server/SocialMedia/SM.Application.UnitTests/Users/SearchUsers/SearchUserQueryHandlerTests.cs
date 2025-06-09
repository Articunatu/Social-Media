using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using SM.Application.Database;
using SM.Application.Users.SearchUsers;
using SM.Domain.Users;

namespace SM.Application.UnitTests.Users.SearchUsers;

public class SearchUserQueryHandlerTests
{
    [Fact]
    public async Task Handle_SearchTextEin_ReturnsNamesAndTagsContainingIt()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        context.Users.AddRange(
            new User { Name = "Heinrich", Tags = new List<string> { "tag1", "einhorn" } },
            new User { Name = "Reinar", Tags = new List<string> { "einmalig" } },
            new User { Name = "Utena", Tags = new List<string> { "rose", "duel" } }
        );
        await context.SaveChangesAsync();

        var handler = new SearchUserQueryHandler(context);
        var query = new SearchUserQuery("ein");

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Contain(new[] { "Heinrich", "Reinar" });
            result.Value.Should().NotContain("Utena");
        }
    }
}
