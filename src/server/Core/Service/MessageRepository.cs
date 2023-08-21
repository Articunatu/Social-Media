using Core.Paging;
using Microsoft.Azure.Cosmos;
using Models.DataTransferObjects;
using Models.Models;
using Models.SubModels.Account;
using Models.SubModels.Message;

namespace Core.Service
{
    public class MessageRepository : IMessageRepository
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

        public async Task<PagedResult<Message>> Get10LatestPostsByAccountId(Guid accountId, string continuationToken = null, int pageSize = 10)
        {
            var query = new QueryDefinition(
                query: "SELECT m.Text, m.Date, m.Reactions, m.Comments FROM Messages m WHERE m.AccountId = @accountId ORDER BY m.Date DESC OFFSET @offset LIMIT @limit")
                .WithParameter("@accountId", accountId)
                .WithParameter("@offset", continuationToken != null ? continuationToken : "")
                .WithParameter("@limit", pageSize + 1); // Fetch one more to check if there are more pages

            var result = await ExecutePagedQuery(query, pageSize);

            return result;
        }

        private async Task<PagedResult<Message>> ExecutePagedQuery(QueryDefinition query, int pageSize)
        {
            var iterator = _container.GetItemQueryIterator<Message>(query);
            var messages = new List<Message>();
            var continuationToken = string.Empty;

            while (iterator.HasMoreResults && messages.Count < pageSize)
            {
                var response = await iterator.ReadNextAsync();
                continuationToken = response.ContinuationToken;

                messages.AddRange(response.ToList());
            }

            if (messages.Count > pageSize)
            {
                messages.RemoveAt(messages.Count - 1); // Remove the extra item used for paging
            }

            return new PagedResult<Message>
            {
                Results = messages,
                ContinuationToken = continuationToken
            };
        }


        public async Task<Message> GetMessageById(Guid id)
        {
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT TOP 1 FROM Message m WHERE m.id = @partitionKey")
                .WithParameter("@partitionKey", id);

            var result = await GetSingle(parameterizedQuery);
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

        public async Task AddReactionToMessage(AccountDto reactor, Message message, Reaction reaction)
        {
            MessageReaction messageReaction = new()
            {
                Reactor = reactor,
                Reaction = reaction.Type
            };

            message.Reactions ??= new List<MessageReaction>();

            message.Reactions.Add(messageReaction);

            await _container.UpsertItemAsync(message);
        }
        
        public async Task AddReactionToComment(AccountDto reactor, Message message, Reaction reaction)
        {
            MessageReaction messageReaction = new()
            {
                Reactor = reactor,
                Reaction = reaction.Type
            };

            message.Reactions ??= new List<MessageReaction>();

            message.Reactions.Add(messageReaction);

            await _container.UpsertItemAsync(message);
        }

        public async Task AddCommentToPost(Post post, M_Comment comment)
        {
            //post.Comments ??= new List<Models.SubModels.Message.Comment>();

            //post.Comments.Add(comment);

            //await _container.UpsertItemAsync(post);
        }

        private async Task<IEnumerable<Message>> GetAll(QueryDefinition query)
        {
            var queryIterator = _container.GetItemQueryIterator<Message>(
                queryDefinition: query
            );

            var result = new List<Message>();

            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                result.AddRange(response.ToList());
            }

            return result;
        }

        private async Task<Message> GetSingle(QueryDefinition query)
        {
            using FeedIterator<Message> filteredFeed = _container.GetItemQueryIterator<Message>(
                queryDefinition: query
            );

            FeedResponse<Message> response = await filteredFeed.ReadNextAsync();
            Message? result = response.FirstOrDefault();

            return result;
        }

        public async Task<IEnumerable<Message>> GetTrendingPosts()
        {
            var query = new QueryDefinition(
                query: "SELECT TOP 10 m.Text, m.Date, COUNT(m.Reactions), COUNT(m.Comments.Count) FROM Message m WHERE m.Date > Time.Now ORDER BY COUNT(m.Reactions) DESC")
            ;

            var result = await GetAll(query);

            return result.ToArray();
        }
    }
}