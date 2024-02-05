using Microsoft.Azure.Cosmos;
using SocialMedia.Domain.Users;
using User = SocialMedia.Domain.Users.User;

namespace SocialMedia.Infrastructure.Repositories
{
    internal sealed class UserReadRepository(CosmosClient cosmosClient) :
    ReadRepository<User, Guid>(cosmosClient.GetContainer("social-media", "Account")), IUserReadRepository
    { }

}
