using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SM.Application.Database;

namespace SM.Application.IntegrationTests.IntegrationAbstractions;

public sealed class IntegrationTestFixture : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"integration-tests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:TokenKey"] = "integration-test-token-key-with-more-than-sixty-four-bytes-for-hmac-sha512-signing"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Register DbContextFactories with in-memory database for testing
            services.AddDbContextFactory<IdentityDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
            services.AddDbContextFactory<ContentDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
            services.AddDbContextFactory<SocialGraphDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }

    public async Task ResetDatabaseAsync()
    {
        var identityFactory = Services.GetRequiredService<IDbContextFactory<IdentityDbContext>>();
        var contentFactory = Services.GetRequiredService<IDbContextFactory<ContentDbContext>>();
        var socialGraphFactory = Services.GetRequiredService<IDbContextFactory<SocialGraphDbContext>>();

        await using var identityContext = await identityFactory.CreateDbContextAsync();
        await using var contentContext = await contentFactory.CreateDbContextAsync();
        await using var socialGraphContext = await socialGraphFactory.CreateDbContextAsync();

        await identityContext.Database.EnsureDeletedAsync();
        await identityContext.Database.EnsureCreatedAsync();
        await contentContext.Database.EnsureCreatedAsync();
        await socialGraphContext.Database.EnsureCreatedAsync();
    }
}
