using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SM.Application.Database;
using SM.Application.IntegrationTests.Users;

namespace SM.Application.IntegrationTests.IntegrationAbstractions;

public abstract class BaseIntegrationTest(IntegrationTestFixture fixture) : IClassFixture<IntegrationTestFixture>, IAsyncLifetime
{
    protected readonly ISender Sender = fixture.Services.GetRequiredService<ISender>();
    protected readonly IDbContextFactory<ApplicationDbContext> DbContextFactory = fixture.Services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    protected ApplicationDbContext DbContext { get; private set; } = default!;
    protected UserTestData Users { get; private set; } = default!;

    public virtual async Task InitializeAsync()
    {
        await fixture.ResetDatabaseAsync();
        DbContext = await DbContextFactory.CreateDbContextAsync();
        Users = new UserTestData(DbContextFactory);
    }

    public virtual async Task DisposeAsync()
    {
        if (DbContext is not null)
        {
            await DbContext.DisposeAsync();
        }
    }
}
