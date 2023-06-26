using Microsoft.Azure.Cosmos;
using Models.DataTransferObjects;
using Models.Models;
using Models.SubModels;
using Models.SubModels.Account;

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

        public async Task AddNewAccount(Account created)
        {
            created.Id = Guid.NewGuid();
            await _container.CreateItemAsync(created);
        }

        public async Task<Account> GetAccountById(Guid id)
        {
            var parameterizedQuery = new QueryDefinition(
            query: "SELECT a.id, a.Tag, a.FirstName, a.LastName, a.ProfilePhoto FROM Account a" +
                "JOIN (SELECT TOP 10 AccountId, MAX(PostDate) AS LatestPostDate" +
                "FROM Post WHERE AccountId = @partitionKey" +
                "GROUP BY AccountId) p ON a.id = p.AccountId" +
                "ORDER BY p.LatestPostDate DESC")
            .WithParameter("@partitionKey", id);

            var result = await GetSingle<Account>(parameterizedQuery);

            return result;
        }

        public async Task<Account?> GetAccountByTag(string tag)
        {
            var parameterizedQuery = new QueryDefinition(
            query: "SELECT TOP 1 a.id, a.Tag FROM Account a WHERE a.Tag = @partitionKey")
            .WithParameter("@partitionKey", tag);

            var result = await GetSingle<Account>(parameterizedQuery);

            return result;
        }

        public async Task<Account> GetAccountByToken(string token)
        {
            var query = $"SELECT id, Token FROM c WHERE c.Token.Text = '{token}'";
            var iterator = _container.GetItemQueryIterator<Account>(query);
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task<IEnumerable<Account>> GetTop10FollowersById(Guid id)
        {
            var parameterizedQuery = new QueryDefinition(
            query: "SELECT TOP 10 Followers FROM Account a WHERE a.Tag = @partitionKey")
            .WithParameter("@partitionKey", id);

            var result = await GetAll<Account>(parameterizedQuery);

            return result;
        }

        public async Task<IEnumerable<Account>> GetTop10FollowedAccountsById(Guid id)
        {
            var parameterizedQuery = new QueryDefinition(
            query: "SELECT TOP 10 Following FROM Account a WHERE a.Tag = @partitionKey")
            .WithParameter("@partitionKey", id);

            var result = await GetAll<Account>(parameterizedQuery);

            return result;
        }

        public async Task AddReactedPostToAccount(ReactedPost reactedPost, Guid accountId)
        {
            Account account = new() { Id = accountId };

            // add code to get accounts reacted posts

            account.ReactedPosts ??= new List<ReactedPost>();

            account.ReactedPosts.Add(reactedPost);

            await _container.UpsertItemAsync(account);
        }

        public async Task AddPostToAccount(Guid accountId, Post post)
        {
            Account account = new() { Id = accountId };
            account.Posts ??= new List<Post>();
            account.Posts.Add(post);

            await _container.UpsertItemAsync(account);
        }

        public async Task AddCommentToAccount(Guid accountId, A_Comment comment)
        {
            Account account = new() { Id = accountId };
            account.Comments ??= new List<A_Comment>();
            account.Comments.Add(comment);

            await _container.UpsertItemAsync(account);
        }

        public async Task UploadPhoto(Photo photo, Guid accountId)
        {
            Account account = new() { Id = accountId };
            account.Photos ??= new List<Photo>();
            account.Photos.Add(photo);

            if (photo.IsProfilePhoto)
            {
                account.ProfilePhoto = photo;
            }
            else if (photo.IsBackgroundPhoto)
            {
                account.BackgroundPhoto = photo;
            }

            await _container.UpsertItemAsync(account);
        }

        public async Task<IEnumerable<Photo>> GetTop10ProfilePhotos(Guid id)
        {
            var parameterizedQuery = new QueryDefinition(
            query: "SELECT TOP 10 a.Photos FROM Account a WHERE a.Tag = @partitionKey")
            .WithParameter("@partitionKey", id);

            var result = await GetAll<Photo>(parameterizedQuery);

            return result;
        }

        private async Task<IEnumerable<T>> GetAll<T>(QueryDefinition query)
        {
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

        private async Task<T> GetSingle<T>(QueryDefinition query)
        {
            using FeedIterator<T> filteredFeed = _container.GetItemQueryIterator<T>(
                queryDefinition: query
            );

            FeedResponse<T> response = await filteredFeed.ReadNextAsync();
            T? result = response.FirstOrDefault();

            return result;
        }
    }
}