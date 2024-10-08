using SocialMedia.Domain.Abstractions;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq.Expressions;

namespace SocialMedia.Infrastructure.Repositories
{
    internal abstract class NoSqlRepository<TItem, TItemId>(Container container)
        where TItem : Entity<TItemId>
    {
        protected readonly Container _container = container;

        public async Task<TProperty?> GetSingle<TProperty>(
            Expression<Func<TItem, bool>>? filter = null,
            Func<IQueryable<TItem>, IQueryable<TProperty>>? queryModifier = null)
        {
            // Retrieve multiple items, allowing for the filter and query modifier
            var items = await GetMultiple<TProperty>(filter, queryModifier);

            // Return the first item if available
            return items.FirstOrDefault();
        }

        public async Task<IEnumerable<TProperty?>> GetMultiple<TProperty>(
            Expression<Func<TItem, bool>>? filter = null,
            Func<IQueryable<TItem>, IQueryable<TProperty>>? queryModifier = null)
        {
            // Ensure TProperty is a class type (optional, but good for safety)
            //if (!typeof(TProperty).IsClass)
            //{
            //    throw new ArgumentException("TProperty must be a class type.");
            //}

            // Create the base query using the LINQ provider for Cosmos DB
            IQueryable<TItem> query = _container.GetItemLinqQueryable<TItem>();

            // Apply filtering if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // If queryModifier is provided, apply it
            IQueryable<TProperty>? modifiedQuery = queryModifier?.Invoke(query);
            if (modifiedQuery != null)
            {
                query = modifiedQuery.Cast<TItem>(); // Keep the base query as TItem for iteration
            }

            var iterator = query.ToFeedIterator();
            var results = new List<TProperty?>();

            // Fetch results from the iterator
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();

                // Check if TProperty exists in TItem and project accordingly
                foreach (var item in response)
                {
                    // Create a new instance of TProperty
                    var propertyInfo = typeof(TItem).GetProperty(typeof(TProperty).Name) ?? 
                        throw new InvalidOperationException($"Property '{typeof(TProperty).Name}' does not exist on type '{typeof(TItem).Name}'.");

                    // Get the value of the property and cast it to TProperty
                    var propertyValue = propertyInfo.GetValue(item);
                    results.Add((TProperty?)propertyValue);
                }
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