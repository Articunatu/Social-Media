using SocialMedia.Domain.Abstractions;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Infrastructure.Repositories;

internal abstract class CommandRepository<TItem, TItemId>(Container container) where TItem : Entity<TItemId>
{
    public async Task Add(TItem item)
    {
        await container.CreateItemAsync(item);
    }

    public async Task Delete(TItemId id)
    {
        var item = await container.GetItemLinqQueryable<TItem>().FirstOrDefaultAsync(t => t.Id!.Equals(id));
        if (item is not null)
        {
            await SoftDeleteCheck(container, item);
            await container.DeleteItemAsync<TItem>(id!.ToString(), new PartitionKey(id.ToString()));
        }
    }

    public async Task Update(TItem item)
    {
        await container.UpsertItemAsync(item);
    }

    private static async Task SoftDeleteCheck(Container container, TItem item)
    {
        if (item is ISoftDeletable softDeletableEntity)
        {
            softDeletableEntity.IsDeleted = true;
            softDeletableEntity.TimeOfDelete = DateTime.UtcNow;
            await container.UpsertItemAsync(softDeletableEntity);
            return;
        }
    }

    public async Task AddMultiple(List<TItem> items)
    {
        List<Task> tasks = new(items.Count);
        foreach (var item in items)
        {
            tasks.Add(container.CreateItemAsync(item, new PartitionKey(item.Id.ToString()))
                .ContinueWith(itemResponse =>
                {
                    if (!itemResponse.IsCompletedSuccessfully)
                    {
                        AggregateException innerExceptions = itemResponse.Exception!.Flatten();
                        if (innerExceptions.InnerExceptions.FirstOrDefault(innerEx => innerEx is CosmosException) is CosmosException cosmosException)
                            Console.WriteLine($"Received {cosmosException.StatusCode} ({cosmosException.Message}).");
                        else
                            Console.WriteLine($"Exception {innerExceptions.InnerExceptions.FirstOrDefault()}.");
                    }
                }));
        }
        await Task.WhenAll(tasks);
    }

    public async Task UpdateMultiple(List<TItem> items)
    {
        List<Task> tasks = new(items.Count);
        foreach (var item in items)
        {
            tasks.Add(container.UpsertItemAsync(item)
                .ContinueWith(itemResponse =>
                {
                    if (!itemResponse.IsCompletedSuccessfully)
                    {
                        AggregateException innerExceptions = itemResponse.Exception!.Flatten();
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