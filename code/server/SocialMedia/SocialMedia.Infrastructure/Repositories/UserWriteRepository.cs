using Microsoft.Azure.Cosmos;
using SocialMedia.Domain.Users;
using User = SocialMedia.Domain.Users.User;

namespace SocialMedia.Infrastructure.Repositories
{
    internal sealed class UserWriteRepository(ApplicationDbContext context, CosmosClient cosmosClient) : 
        WriteRepository<User, Guid>(context, cosmosClient.GetContainer("social-media", "Account")), IUserWriteRepository { }
}
