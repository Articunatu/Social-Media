using SocialMedia.Domain.Abstractions;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Infrastructure.Repositories
{
    internal abstract class ReadRepository<TItem, TItemId>(Container container)
        where TItem : Entity<TItemId>
    {
        protected readonly Container _container = container;

        public async Task<TItem?> GetSingle(object key, string query)
        {
            var parameterizedQuery = new QueryDefinition(query).WithParameter("@partitionKey", key);
            using FeedIterator<TItem?> filteredFeed = _container.GetItemQueryIterator<TItem?>(parameterizedQuery);
            FeedResponse<TItem?> response = await filteredFeed.ReadNextAsync();
            TItem? item = response.FirstOrDefault();
            return item;
        }

        public async Task<IEnumerable<TItem?>> GetMultiple(object key, string query)
        {
            var parameterizedQuery = new QueryDefinition(query).WithParameter("@partitionKey", key);
            var queryIterator = _container.GetItemQueryIterator<TItem?>(parameterizedQuery);
            var items = new List<TItem?>();
            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                items.AddRange([.. response]);
            }
            return items;
        }

        public async Task Add(TItem item)
        {
            await _container.CreateItemAsync(item);
        }

        public async Task AddMultiple(List<TItem> items)
        {
            List<Task> tasks = new(items.Count);
            foreach (var item in items)
            {
                tasks.Add(_container.CreateItemAsync(item, new PartitionKey(item.Id.ToString()))
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
            await Task.WhenAll(tasks);
        }

        public async Task Delete(TItemId id)
        {
            var item = await _container.GetItemLinqQueryable<TItem>().FirstOrDefaultAsync(t => t.Id.Equals(id));
            if (item is not null)
            {
                if (item is ISoftDeletable softDeletableEntity)
                {
                    softDeletableEntity.IsDeleted = true;
                    softDeletableEntity.TimeOfDelete = DateTime.UtcNow;
                    await _container.UpsertItemAsync(softDeletableEntity);
                }
                else
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    await _container.DeleteItemAsync<TItem>(id.ToString(), new PartitionKey(id.ToString()));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
        }

        public async Task Update(TItem item)
        {
            await _container.UpsertItemAsync(item);
        }
    }
}