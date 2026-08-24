using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SM.Application.Database;
using SM.Application.IntegrationTests.Users;

namespace SM.Application.IntegrationTests.IntegrationAbstractions;

public abstract class BaseIntegrationTest(IntegrationTestFixture fixture) : IClassFixture<IntegrationTestFixture>, IAsyncLifetime
{
    protected readonly ISender Sender = fixture.Services.GetRequiredService<ISender>();
    protected readonly IDbContextFactory<IdentityDbContext> IdentityDbContextFactory = fixture.Services.GetRequiredService<IDbContextFactory<IdentityDbContext>>();
    protected readonly IDbContextFactory<ContentDbContext> ContentDbContextFactory = fixture.Services.GetRequiredService<IDbContextFactory<ContentDbContext>>();
    protected readonly IDbContextFactory<SocialGraphDbContext> SocialGraphDbContextFactory = fixture.Services.GetRequiredService<IDbContextFactory<SocialGraphDbContext>>();
    protected readonly IDbContextFactory<FeedDbContext> FeedDbContextFactory = fixture.Services.GetRequiredService<IDbContextFactory<FeedDbContext>>();

    protected IdentityDbContext IdentityDbContext { get; private set; } = default!;
    protected ContentDbContext ContentDbContext { get; private set; } = default!;
    protected SocialGraphDbContext SocialGraphDbContext { get; private set; } = default!;
    protected FeedDbContext FeedDbContext { get; private set; } = default!;
    protected UserTestData Users { get; private set; } = default!;

    public virtual async Task InitializeAsync()
    {
        await fixture.ResetDatabaseAsync();
        IdentityDbContext = await IdentityDbContextFactory.CreateDbContextAsync();
        ContentDbContext = await ContentDbContextFactory.CreateDbContextAsync();
        SocialGraphDbContext = await SocialGraphDbContextFactory.CreateDbContextAsync();
        FeedDbContext = await FeedDbContextFactory.CreateDbContextAsync();
        Users = new UserTestData(IdentityDbContextFactory, ContentDbContextFactory);
    }

    public virtual async Task DisposeAsync()
    {
        if (IdentityDbContext is not null)
        {
            await IdentityDbContext.DisposeAsync();
        }

        if (ContentDbContext is not null)
        {
            await ContentDbContext.DisposeAsync();
        }

        if (SocialGraphDbContext is not null)
        {
            await SocialGraphDbContext.DisposeAsync();
        }

        if (FeedDbContext is not null)
        {
            await FeedDbContext.DisposeAsync();
        }
    }
}
