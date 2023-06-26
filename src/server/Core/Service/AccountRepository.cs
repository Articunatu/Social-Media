using Microsoft.Azure.Cosmos;
using Models.Models;
using Models.SubModels;
using Models.SubModels.Account;

namespace Core.Service
{
    public class AccountRepository : IAccountRepository
    {
        readonly Container _container;
        readonly IGenericQuery _query;
        readonly string containerName = "Account";
        string query2 = "SELECT VALUE a.lastName\r\nFROM Account a\r\nWHERE IS_DEFINED(a.lastName)\r\n";

        public AccountRepository(CosmosClient client, string databaseName, IGenericQuery query)
        {
            _container = client.GetContainer(databaseName, containerName);
            _query = query;
        }

        public async Task AddNewAccount(Account created)
        {
            created.Id = Guid.NewGuid();
            await _container.CreateItemAsync(created);
        }

        public async Task<Account> GetAccountById(Guid id)
        {
            var parameterizedQuery = new QueryDefinition(
            query: "SELECT TOP 1 * FROM Account a WHERE a.Tag = @partitionKey")
            .WithParameter("@partitionKey", id);

            var result = await _query.GetSingle<Account>(parameterizedQuery, containerName);

            return result;
        }

        public async Task<Account?> GetAccountByTag(string tag)
        {
            var parameterizedQuery = new QueryDefinition(
            query: "SELECT TOP 1 * FROM Account a WHERE a.Tag = @partitionKey")
            .WithParameter("@partitionKey", tag);

            var result = await _query.GetSingle<Account>(parameterizedQuery, containerName);

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

            var result = await _query.GetAll<Account>(parameterizedQuery, containerName);

            return result;
        }

        public async Task<IEnumerable<Account>> GetTop10FollowedAccountsById(Guid id)
        {
            var parameterizedQuery = new QueryDefinition(
            query: "SELECT TOP 10 Following FROM Account a WHERE a.Tag = @partitionKey")
            .WithParameter("@partitionKey", id);

            var result = await _query.GetAll<Account>(parameterizedQuery, containerName);

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
    }
}