using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(
                eventData, result, cancellationToken);
        }

        IEnumerable<EntityEntry<ISoftDeletable>> entries =
            eventData
                .Context
                .ChangeTracker
                .Entries<ISoftDeletable>()
                .Where(e => e.State == EntityState.Deleted);

        foreach (EntityEntry<ISoftDeletable> softDeletable in entries)
        {
            softDeletable.State = EntityState.Modified;
            softDeletable.Entity.IsDeleted = true;
            softDeletable.Entity.TimeOfDelete = DateTime.UtcNow;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}


public class CosmosDbContainerWrapper<TEntity> where TEntity : class
{
    private readonly Container _container;

    public CosmosDbContainerWrapper(Container container)
    {
        _container = container;
    }

    public async Task DeleteItemAsync(string id)
    {
        // Intercept delete operation and mark entity as soft deleted
        var entity = await _container.ReadItemAsync<TEntity>(id, new PartitionKey(id));
        if (entity != null)
        {
            if (entity is ISoftDeletable softDeletableEntity)
            {
                softDeletableEntity. = true;
                softDeletableEntity.TimeOfDelete = DateTime.UtcNow;
                await _container.UpsertItemAsync(entity);
            }
            else
            {
                await _container.DeleteItemAsync<TEntity>(id, new PartitionKey(id));
            }
        }
        else
        {
            throw new InvalidOperationException($"Entity with ID {id} does not exist.");
        }
    }
}