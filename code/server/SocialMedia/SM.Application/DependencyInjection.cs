using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SM.Application.Database;

namespace SM.Application;

public static class DependencyInjection
{
    public static void SeedDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var identityFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<IdentityDbContext>>();
        var contentFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ContentDbContext>>();
        var socialGraphFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SocialGraphDbContext>>();
        var feedFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FeedDbContext>>();
        var mediaFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MediaDbContext>>();

        using var identityContext = identityFactory.CreateDbContext();
        using var contentContext = contentFactory.CreateDbContext();
        using var socialGraphContext = socialGraphFactory.CreateDbContext();
        using var feedContext = feedFactory.CreateDbContext();
        using var mediaContext = mediaFactory.CreateDbContext();

        feedContext.Database.Migrate();
        ApplicationDbContextSeeder.Seed(identityContext, contentContext, socialGraphContext, mediaContext);
    }
}
