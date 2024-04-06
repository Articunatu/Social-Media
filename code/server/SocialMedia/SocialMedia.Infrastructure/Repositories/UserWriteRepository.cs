using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Users;
using User = SocialMedia.Domain.Users.User;

namespace SocialMedia.Infrastructure.Repositories
{
    internal sealed class UserWriteRepository(ApplicationDbContext context, CosmosClient cosmosClient) : 
        WriteRepository<User, Guid>(context, cosmosClient.GetContainer("social-media", "Account")), IUserWriteRepository 
    {
        public async Task Follow(Guid followerId, Guid followingdId)
        {
            var follow = new FollowUser(followerId, followingdId);
            _dbContext.Follows.Add(follow);
            await FollowCosmos(followerId, followingdId, follow);
        }

        private async Task FollowCosmos(Guid followerId, Guid followingdId, FollowUser follow)
        {
            var follower = await _container.GetItemLinqQueryable<User>().FirstOrDefaultAsync(x => x.Id == followerId);
            var following = await _container.GetItemLinqQueryable<User>().FirstOrDefaultAsync(x => x.Id == followingdId);
            follower.Following.Add(follow);
            following.Following.Add(follow);
            await _container.UpsertItemAsync(follower);
            await _container.UpsertItemAsync(following);
        }
    }
}
