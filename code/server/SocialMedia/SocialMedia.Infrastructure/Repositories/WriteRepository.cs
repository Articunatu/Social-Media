using Microsoft.Azure.Cosmos;
using EFCore.BulkExtensions;
using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Infrastructure.Repositories
{
    internal abstract class WriteRepository<TEntity, TEntityId>(
        ApplicationDbContext dbContext, 
        Container container)
        where TEntity : Entity<TEntityId>
    {
        protected readonly ApplicationDbContext table = dbContext;
        protected readonly Container doc = container;

        public async Task Add(TEntity entity)
        {
            await table.Set<TEntity>().AddAsync(entity);
            await doc.CreateItemAsync(entity);
        }

        public async Task AddMultiple(List<TEntity> entities)
        {
            await table.BulkInsertAsync(entities);
            await BulkInsertCosmos(entities);
        }

        private async Task BulkInsertCosmos(List<TEntity> entities)
        {
            List<Task> tasks = new List<Task>(entities.Count());
            foreach (var item in entities)
            {
                tasks.Add(doc.CreateItemAsync(item, new PartitionKey(item.Id.ToString()))
                    .ContinueWith(itemResponse =>
                    {
                        if (!itemResponse.IsCompletedSuccessfully)
                        {
                            AggregateException innerExceptions = itemResponse.Exception.Flatten();
                            if (innerExceptions.InnerExceptions.FirstOrDefault(innerEx => innerEx is CosmosException) is CosmosException cosmosException)
                                Console.WriteLine($"Received {cosmosException.StatusCode} ({cosmosException.Message}).");
                            else
                                Console.WriteLine($"Exception {innerExceptions.InnerExceptions.FirstOrDefault()}.");
                        }
                    }));
            }
            // Wait until all are done
            await Task.WhenAll(tasks);
        }

        public async Task Delete(TEntityId id)
        {
            var entity = await table.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                table.Set<TEntity>().Remove(entity);
                await RemoveCosmos(id, entity);
            }
        }

        private async Task RemoveCosmos(TEntityId id, TEntity? entity)
        {
            if (entity is ISoftDeletable softDeletableEntity)
            {
                softDeletableEntity.IsDeleted = true;
                softDeletableEntity.TimeOfDelete = DateTime.UtcNow;
                await doc.UpsertItemAsync(softDeletableEntity);
            }
            else
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                await doc.DeleteItemAsync<TEntity>(id.ToString(), new PartitionKey(id.ToString()));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public async Task Update(TEntity entity)
        {
            table.Set<TEntity>().Update(entity);
            await doc.UpsertItemAsync(entity);
        }
    }
}
