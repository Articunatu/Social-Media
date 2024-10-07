using SocialMedia.Domain.Abstractions;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq.Expressions;
using NetTopologySuite.Index.HPRtree;

namespace SocialMedia.Infrastructure.Repositories
{
    internal abstract class NoSqlRepository<TItem, TItemId>(Container container)
        where TItem : Entity<TItemId>
    {
        protected readonly Container _container = container;

        public async Task<TItem?> GetSingle(
            Expression<Func<TItem, bool>>? filter = null,
            Func<IQueryable<TItem>, IQueryable<TItem>>? queryModifier = null)
        {
            // Create the base query using the LINQ provider for Cosmos DB
            IQueryable<TItem> query = _container.GetItemLinqQueryable<TItem>();

            // Apply filtering if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply any additional query modifications (e.g., Select, Take, etc.)
            if (queryModifier != null)
            {
                query = queryModifier(query);
            }
            else
            {
                // Ensure we only retrieve one item
                query = query.Take(1);
            }

            var iterator = query.ToFeedIterator();

            // Retrieve the first result from the query iterator
            if (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                return response.FirstOrDefault();
            }

            return default; // Return null or default if no items are found
        }


        public async Task<IEnumerable<TItem?>> GetMultiple(
            Expression<Func<TItem, bool>>? filter = null,
            Func<IQueryable<TItem>, IQueryable<TItem>>? queryModifier = null)
        {
            // Create the base query using the LINQ provider for Cosmos DB
            IQueryable<TItem> query = _container.GetItemLinqQueryable<TItem>();

            // Apply filtering if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply any additional query modifications (like Select, Take, etc.)
            if (queryModifier != null)
            {
                query = queryModifier(query);
            }

            var iterator = query.ToFeedIterator();
            var results = new List<TItem?>();

            // Fetch all results from the iterator
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
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
        
        public async Task UpdateMultiple(List<TItem> items)
        {
            List<Task> tasks = new(items.Count);
            foreach (var item in items)
            {
                tasks.Add(_container.UpsertItemAsync(item)
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
    }
}