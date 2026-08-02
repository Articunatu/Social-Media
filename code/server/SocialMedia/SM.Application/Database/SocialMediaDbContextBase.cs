using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Domain.Abstractions;

namespace SM.Application.Database;

public abstract class SocialMediaDbContextBase : DbContext
{
    private readonly IMediator _mediator;

    protected SocialMediaDbContextBase(DbContextOptions options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
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
}
