using Microsoft.Azure.Cosmos;
using Models.DataTransferObjects;
using Models.Models;
using Models.SubModels.Account;
using Models.SubModels.Message;
using System.Numerics;

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

        public async Task<IEnumerable<Message>> ReadTop5LatestMessagesFromAccountById(Guid id)
        {
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT TOP 5 m.text, m.date, m.reactionlist, m.commentlist, m.sharecount FROM Account a WHERE a.id = @partitionKey ORDER BY m.date DESC")
            .WithParameter("@partitionKey", id);

            var queryIterator = _container.GetItemQueryIterator<Message>(
                queryDefinition: parameterizedQuery
            );

            var result = new List<Message>();

            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                result.AddRange(response.ToList());
            }

            return result;
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

        public async Task AddCommentToPost(Post post, Models.SubModels.Message.M_Comment comment)
        {
            //post.Comments ??= new List<Models.SubModels.Message.Comment>();

            //post.Comments.Add(comment);

            //await _container.UpsertItemAsync(post);
        }
    }
}