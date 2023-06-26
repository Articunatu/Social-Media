using Microsoft.Azure.Cosmos;
using Models.Models;

namespace Core.Service
{
    public class GenericQuery : IGenericQuery
    {
        readonly Container _container;
        string _containerName = "";

        public GenericQuery(CosmosClient client, string databaseName)
        {
            _container = client.GetContainer(databaseName, _containerName);
        }

        public async Task<IEnumerable<T>> GetAll<T>(QueryDefinition query, string containerName)
        {
            _containerName = containerName;
            var queryIterator = _container.GetItemQueryIterator<T>(
                queryDefinition: query
            );

            var result = new List<T>();

            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                result.AddRange(response.ToList());
            }

            return result;
        }

        public async Task<T> GetSingle<T>(QueryDefinition query, string containerName)
        {
            _containerName = containerName;
            using FeedIterator<T> filteredFeed = _container.GetItemQueryIterator<T>(
                queryDefinition: query
            );

            FeedResponse<T> response = await filteredFeed.ReadNextAsync();
            T? result = response.FirstOrDefault();

            return result;
        }
    }
}
