using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SM.Application.Database;

namespace SM.Application;

public static class DependencyInjection
{
    // ...existing code...
    public static void SeedDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var identityFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<IdentityDbContext>>();
        var contentFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ContentDbContext>>();
        var socialGraphFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SocialGraphDbContext>>();

        using var identityContext = identityFactory.CreateDbContext();
        using var contentContext = contentFactory.CreateDbContext();
        using var socialGraphContext = socialGraphFactory.CreateDbContext();

        ApplicationDbContextSeeder.Seed(identityContext, contentContext, socialGraphContext);
    }
    // ...existing code...
}
