using Models.Models;
using Microsoft.Azure.Cosmos;

namespace Core.Service
{
    public class ReactionRepository : IReactionRepository
    {
        readonly Container _container;

        public ReactionRepository(CosmosClient client, string databaseName)
        {
            _container = client.GetContainer(databaseName, "Reaction");
        }

        public async Task Create(Reaction created)
        {
            await _container.CreateItemAsync(created);
        }

        public async Task<IEnumerable<Reaction>> ReadAll()
        {
            string query = "SELECT * FROM c";

            var selected = _container.GetItemQueryIterator<Reaction>(new QueryDefinition(query));

            List<Reaction> result = new();
            while (selected.HasMoreResults)
            {
                var response = await selected.ReadNextAsync();
                result.AddRange(response);
            }

            return result.ToArray();
        }

        public async Task<Reaction> ReadSingle(int id)
        {
            // Read existing item from container
            //var account = (await ReadAll(id)).FirstOrDefault(a => a.Id.Equals(id));
            //return account;
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT TOP 1 FROM Reaction r WHERE r.type = @partitionKey")
                .WithParameter("@partitionKey", id);

            // Query multiple items from container
            using FeedIterator<Reaction> filteredFeed = _container.GetItemQueryIterator<Reaction>(
                queryDefinition: parameterizedQuery
            );

            Reaction? result = new();

            // Iterate query result pages
            while (filteredFeed.HasMoreResults)
            {
                FeedResponse<Reaction> response = await filteredFeed.ReadNextAsync();
                result = response.FirstOrDefault() ?? result;
            }
            return result;
        }

        public async Task Update(Reaction updated)
        {
            await _container.UpsertItemAsync(updated);
        }

        public async Task Delete(int id)
        {
            var newId = id.ToString();
            await _container.DeleteItemAsync<Reaction>(newId, new PartitionKey(newId));
        }
    }
}