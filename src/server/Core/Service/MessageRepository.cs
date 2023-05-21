using Core.Paging;
using Microsoft.Azure.Cosmos;
using Models.Models;
using Models.SubModels.Account;

namespace Core.Service
{
    public class MessageRepository : IMessageRepository
    {
        readonly Container _container;
        readonly string containerName = "Message";
        readonly IAccountRepository _accountRepository;
        const int PAGE_SIZE = 10;

        public MessageRepository(CosmosClient client, string databaseName, IAccountRepository accountRepository)
        {
            _container = client.GetContainer(databaseName, containerName);
            this._accountRepository = accountRepository; 
        }

        public async Task Create(Post created, Guid accountId)
        {
            created.Id = Guid.NewGuid();
            await _container.CreateItemAsync(created);
            var account = await _accountRepository.GetAccount(accountId);
            account.Posts.Add(created);
            await _accountRepository.Update(account);
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

        public async Task<Message?> GetMessagebyId(Guid id)
        {
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT TOP 1 * FROM Message m WHERE m.id = @partitionKey")
                .WithParameter("@partitionKey", id);

            using FeedIterator<Message> filteredFeed = _container.GetItemQueryIterator<Message>(
                queryDefinition: parameterizedQuery
            );

            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<Message> response = await filteredFeed.ReadNextAsync();
                return response.FirstOrDefault();
            }

            return null;
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

        public async Task<PagedResult<Message>> GetTop10NewestMessagesFromAccount(Guid accountId, string? continuationToken = null)
        {
            var parameterizedQuery = new QueryDefinition(
            query: "SELECT TOP @pageSize Posts FROM Account a WHERE a.id = @partitionKey ORDER BY a.date ASC")
            .WithParameter("@partitionKey", accountId)
            .WithParameter("@pageSize", PAGE_SIZE);

            var result = new List<Message>();
            using var filteredFeed = _container.GetItemQueryIterator<Message>(
                queryDefinition: parameterizedQuery,
                continuationToken: continuationToken,
                requestOptions: new QueryRequestOptions { MaxItemCount = PAGE_SIZE }
            );

            var response = await filteredFeed.ReadNextAsync();
            result.AddRange(response);

            return new PagedResult<Message>
            {
                Results = result,
                ContinuationToken = response.ContinuationToken
            };
        }
    }
}
