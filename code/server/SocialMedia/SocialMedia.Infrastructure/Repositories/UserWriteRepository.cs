using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Users;

namespace SocialMedia.Infrastructure.Repositories
{
    internal sealed class UserWriteRepository(ApplicationDbContext context, CosmosClient cosmosClient) :
        WriteRepository<UserRelational, Guid, UserNoSql>(context, cosmosClient.GetContainer("social-media", "Account")), IUserWriteRepository
    {
        public async Task Follow(Guid followerId, Guid followingdId, UserDTO item)
        {
            await doc.UpsertItemAsync(item);
            var followerRelational = await table.Users.FirstOrDefaultAsync(u => u.Id == followerId);
            await table.Users.FirstOrDefaultAsync(following => following.Id == followerId).Result.Following.Add(followerRelational);
        }
    }
}
