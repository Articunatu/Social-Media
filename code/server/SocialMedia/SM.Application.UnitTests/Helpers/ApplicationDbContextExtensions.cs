using Microsoft.EntityFrameworkCore;
using NSubstitute;
using SM.Application.Database;

namespace SM.Application.UnitTests.Helpers;

public static class ApplicationDbContextExtensions
{
    public static IDbContextFactory<ApplicationDbContext> CreateSubstituteFactory(this ApplicationDbContext context)
    {
        var factory = Substitute.For<IDbContextFactory<ApplicationDbContext>>();
        factory.CreateDbContextAsync(Arg.Any<CancellationToken>())
               .Returns(Task.FromResult(context));
        return factory;
    }
}
