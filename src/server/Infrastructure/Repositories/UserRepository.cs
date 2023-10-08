using Domain.Repositories;
using Infrastructure.Abstractions;
using Microsoft.Azure.Cosmos;
using User = Domain.Entities.User;

namespace Infrastructure.Repositories
{
    public sealed class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CosmosClient client, string databaseName, string containerName)
            : base(client, databaseName, containerName)
        {
        }

        public async Task<User> GetUserById(Guid userId, CancellationToken cancellationToken)
        {
            QueryDefinition query = new("SELECT Id, Tag, FirstName, LastName FROM User");

            var user = (await Get(query, userId)).Single();

            return user;
        }

        public Task<User> GetUserByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsEmailUnique(string email)
        {
             throw new NotImplementedException();
        }
    }
}