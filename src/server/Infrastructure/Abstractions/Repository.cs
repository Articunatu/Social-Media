using Domain.Entities;
using Domain.Primitives;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Abstractions
{
    public abstract class Repository<TEntity>
        where TEntity : Entity
    {
        protected readonly Container _container;

        protected Repository(CosmosClient client, string databaseName, string containerName)
        {
            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task<IEnumerable<TEntity>> Get(QueryDefinition query, object key)
        {
            var results = await _container.GetItemQueryIterator<TEntity>(query).ReadNextAsync();
            return results;
        }

        public async Task Add(TEntity entity)
        {
            await _container.CreateItemAsync(entity);
        }

        public async Task Edit(TEntity entity)
        {
            await _container.UpsertItemAsync(entity);
        }

        public async Task Remove(TEntity entity)
        {
            var newId = entity.Id.ToString();
            await _container.DeleteItemAsync<TEntity>(newId, new PartitionKey(newId));
        }
    }
}