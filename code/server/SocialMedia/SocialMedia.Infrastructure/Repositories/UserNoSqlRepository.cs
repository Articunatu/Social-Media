using Microsoft.Azure.Cosmos;
using SocialMedia.Domain.Users;

namespace SocialMedia.Infrastructure.Repositories
{
    internal sealed class UserNoSqlRepository(CosmosClient cosmosClient) :
    NoSqlRepository<UserNoSql, Guid>(cosmosClient.GetContainer("social-media", "Account")), IUserNoSqlRepository
    {
        public Task Follow(Guid followerId, Guid followingId, UserDTO item)
        {
            throw new NotImplementedException();
        }
    }
}
