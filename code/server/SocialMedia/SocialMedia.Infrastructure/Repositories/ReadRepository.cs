using Microsoft.Azure.Cosmos;
using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Infrastructure.Repositories
{
    internal abstract class ReadRepository<TEntity, TEntityId>(Container container)
        where TEntity : Entity<TEntityId>
    {
        protected readonly Container _container = container;

        public async Task<IEnumerable<TEntity?>> GetMultiple<TEntity>(object key, string query)
        {
            var parameterizedQuery = new QueryDefinition(query).WithParameter("@partitionKey", key);
            var queryIterator = _container.GetItemQueryIterator<TEntity?>(parameterizedQuery);
            var result = new List<TEntity?>();
            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                result.AddRange([.. response]);
            }
            return result;
        }

        public async Task<TEntity?> GetSingle<TEntity>(object key, string query)
        {
            var parameterizedQuery = new QueryDefinition(query).WithParameter("@partitionKey", key);
            using FeedIterator<TEntity?> filteredFeed = _container.GetItemQueryIterator<TEntity?>(parameterizedQuery);
            FeedResponse<TEntity?> response = await filteredFeed.ReadNextAsync();
            TEntity? result = response.FirstOrDefault();
            return result;
        }
    }
}
