using Microsoft.Azure.Cosmos;
using Models.DataTransferObjects;
using Models.Models;
using Models.SubModels;
using Models.SubModels.Account;
using Models.SubModels.Message;
using System.Security.Principal;

namespace Core.Service
{
    public class AccountRepository : IAccountRepository
    {
        readonly Container _container;
        readonly string containerName = "Account";
        string query2 = "SELECT VALUE a.lastName\r\nFROM Account a\r\nWHERE IS_DEFINED(a.lastName)\r\n";

        public AccountRepository(CosmosClient client, string databaseName)
        {
            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task Create(Account created)
        {
            created.Id = Guid.NewGuid();
            await _container.CreateItemAsync(created);
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

        public async Task<IEnumerable<Account>> ReadAccountsFollowingById(Guid id)
        {
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT a.Following FROM Account a WHERE a.id @partitionKey")
                .WithParameter("@partitionKey", id);

            var queryIterator = _container.GetItemQueryIterator<Account>(
                queryDefinition: parameterizedQuery
            );

            var result = new List<Account>();

            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                result.AddRange(response.ToList());
            }

            return result;
        }

        public async Task<IEnumerable<Account>> ReadAccountsFollowersById(Guid id)
        {
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT a.Followers FROM Account a WHERE a.id @partitionKey")
                .WithParameter("@partitionKey", id);

            var queryIterator = _container.GetItemQueryIterator<Account>(
                queryDefinition: parameterizedQuery
            );

            var result = new List<Account>();

            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                result.AddRange(response.ToList());
            }

            return result;
        }

        public async Task AddReactedPostToAccount(ReactedPost reactedPost, Account account)
        {
            account.ReactedPosts ??= new List<ReactedPost>();

            account.ReactedPosts.Add(reactedPost);

            await _container.UpsertItemAsync(account);
        }

        public async Task AddPostToAccount(Account account, Post post)
        {
            account.Posts??= new List<Post>();

            account.Posts.Add(post);

            await _container.UpsertItemAsync(account);
        }
        
        public async Task AddCommentToAccount(Account account, Models.SubModels.Account.Comment comment)
        {
            account.Comments ??= new List<Models.SubModels.Account.Comment>();
            account.Comments.Add(comment);

            await _container.UpsertItemAsync(account);
        }

        public async Task UploadPhoto(Photo photo, Account account)
        {
            account.Photos ??= new List<Photo>();
            account.Photos.Add(photo);

            if (photo.IsProfilePhoto)
            {
                account.ProfilePicture = photo;
            }

            if (photo.IsBackgroundPhoto)
            {
                account.BackgroundPicture = photo;
            }

            await _container.UpsertItemAsync(account);
        }
    }
}