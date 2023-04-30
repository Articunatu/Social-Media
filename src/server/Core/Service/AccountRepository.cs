using Microsoft.Azure.Cosmos;
using Models.DataTransferObjects;
using Models.Models;

namespace Core.Service
{
    public class AccountRepository : IRepository<Account>
    {
        readonly Container _container;
        readonly string containerName = "Account";

        public AccountRepository(CosmosClient client, string databaseName)
        {
            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task Create(Account created)
        {
            created.Id = Guid.NewGuid();
            await _container.CreateItemAsync(created);
        }
        public async Task<IEnumerable<Account>> ReadAll()
        {
            string query = "SELECT * FROM c";

            var selected = _container.GetItemQueryIterator<Account>(new QueryDefinition(query));

            List<Account> result = new();
            while (selected.HasMoreResults)
            {
                var response = await selected.ReadNextAsync();
                result.AddRange(response);
            }

            return result.ToArray();
        }

        public async Task<Account> ReadSingle(Guid id)
        {
            // Read existing item from container
            //var account = (await ReadAll(id)).FirstOrDefault(a => a.Id.Equals(id));
            //return account;
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT TOP 1 * FROM Account a WHERE a.id = @partitionKey")
                .WithParameter("@partitionKey", id);

            // Query multiple items from container
            using FeedIterator<Account> filteredFeed = _container.GetItemQueryIterator<Account>(
                queryDefinition: parameterizedQuery
            );

            Account? result = new();

            // Iterate query result pages
            while (filteredFeed.HasMoreResults)
            {
                FeedResponse<Account> response = await filteredFeed.ReadNextAsync();
                result = response.FirstOrDefault() ?? result;
            }
            return result;
        }

        public async Task Update(Account updated)
        {
            await _container.UpsertItemAsync(updated);
        }

        public async Task Delete(Guid id)
        {
            var newId = id.ToString();
            await _container.DeleteItemAsync<Account>(newId, new PartitionKey(newId));
        }
    }
}
