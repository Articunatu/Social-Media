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
        using ApplicationDbContext context = ArrangeDatabase();
        context.Users.AddRange(
            new User(Guid.NewGuid(), "bkc_nr1", "Heinrich", "Lunge"),
            new User(Guid.NewGuid(), "solid_warrior", "Reinar", "Braunn"),
            new User(Guid.NewGuid(), "rose_duelist", "Utena", "Tenjou")
        );
        await context.SaveChangesAsync();
        var handler = new SearchUserQueryHandler(context);
        var query = new SearchUserQuery("ein");

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Contain(x =>
                x.FullName.Contains("ein", StringComparison.OrdinalIgnoreCase)
            );

            result.Value.Should().NotContain(x => x.FullName == "Utena");
        }
    }

    private static ApplicationDbContext ArrangeDatabase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new ApplicationDbContext(options);
        return context;
    }
}
