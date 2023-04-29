using Microsoft.Azure.Cosmos;
using Models.Models;

namespace Core.Service
{
    public class MessageRepository : IRepository<Message>
    {
        readonly Container _container;
        readonly string containerName = "Message";

        public MessageRepository(CosmosClient client, string databaseName)
        {
            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task Create(Message created)
        {
            created.Id = Guid.NewGuid();
            await _container.CreateItemAsync(created);
        }
        public async Task<IEnumerable<Message>> ReadAll()
        {
            string query = "SELECT * FROM c";

            var selected = _container.GetItemQueryIterator<Message>(new QueryDefinition(query));

            List<Message> result = new();
            while (selected.HasMoreResults)
            {
                var response = await selected.ReadNextAsync();
                result.AddRange(response);
            }

            return result.ToArray();
        }

        public async Task<Message> ReadSingle(Guid id)
        {
            // Read existing item from container
            //var account = (await ReadAll(id)).FirstOrDefault(a => a.Id.Equals(id));
            //return account;
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT TOP 1 FROM Message m WHERE m.id = @partitionKey")
                .WithParameter("@partitionKey", id);

            // Query multiple items from container
            using FeedIterator<Message> filteredFeed = _container.GetItemQueryIterator<Message>(
                queryDefinition: parameterizedQuery
            );

            Message? result = new();

            // Iterate query result pages
            while (filteredFeed.HasMoreResults)
            {
                FeedResponse<Message> response = await filteredFeed.ReadNextAsync();
                result = response.FirstOrDefault() ?? result;
            }
            return result;
        }

        public async Task Update(Message updated)
        {
            await _container.UpsertItemAsync(updated);
        }

        public async Task Delete(Guid id)
        {
            var newId = id.ToString();
            await _container.DeleteItemAsync<Message>(newId, new PartitionKey(newId));
        }
    }
}
