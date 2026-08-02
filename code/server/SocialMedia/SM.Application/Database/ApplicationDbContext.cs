using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Domain.Abstractions;
using SM.Domain.Authentication;
using SM.Domain.Content;
using SM.Domain.Photos;
using SM.Domain.SocialGraph;
using SM.Domain.Users;

namespace SM.Application.Database;

public class ApplicationDbContext(DbContextOptions options, IMediator mediator) : DbContext(options)
{
    private readonly IMediator _mediator = mediator;

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Post> Posts { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<Reaction> Reactions { get; set; } = default!;
    public DbSet<Token> Tokens { get; set; } = default!;
    public DbSet<Photo> Photos { get; set; } = default!;
    public DbSet<Follow> Follows { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        NameTablesByEntities(builder);
        ApplySoftDeleteFilter(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var result = base.SaveChanges(acceptAllChangesOnSuccess);
        DispatchDomainEventsAsync(CancellationToken.None).GetAwaiter().GetResult();
        return result;
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        await DispatchDomainEventsAsync(cancellationToken);
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEntities = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.GetDomainEvents().Any())
            .ToArray();

        var domainEvents = domainEntities
            .SelectMany(entity => entity.GetDomainEvents())
            .ToArray();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        foreach (var entity in domainEntities)
        {
            entity.ClearDomainEvents();
        }
    }

    private static void NameTablesByEntities(ModelBuilder builder)
    {
        builder.Ignore<AuthoredContent>();
        builder.Entity<Post>().ToTable(nameof(Posts));
        builder.Entity<Comment>().ToTable(nameof(Comments));
    }

    private static void ApplySoftDeleteFilter(ModelBuilder builder)
    {
        builder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        builder.Entity<Post>().HasQueryFilter(m => !m.IsDeleted);
        builder.Entity<Comment>().HasQueryFilter(m => !m.IsDeleted);
    }
}
