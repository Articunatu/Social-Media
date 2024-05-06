using Microsoft.Azure.Cosmos;
using SocialMedia.Domain.Users;
using User = SocialMedia.Domain.Users.User;

namespace SocialMedia.Infrastructure.Repositories
{
    internal sealed class UserWriteRepository(ApplicationDbContext context, CosmosClient cosmosClient) : 
        WriteRepository<User, Guid>(context, cosmosClient.GetContainer("social-media", "Account")), IUserWriteRepository 
    {
        //public async Task Follow(Guid followerId, Guid followingdId)
        //{
        //    var follower = await _container.GetItemLinqQueryable<User>().FirstOrDefaultAsync(x => x.Id == followerId);
        //    var following = await _container.GetItemLinqQueryable<User>().FirstOrDefaultAsync(x => x.Id == followingdId);
        //    var follow = new FollowUser(followerId, followingdId, follower, following);
        //    _dbContext.Follows.Add(follow);
        //    await FollowCosmos(followerId, followingdId, follow, follower, following);
        //}

        //private async Task FollowCosmos(Guid followerId, Guid followingdId, FollowUser follow, User follower, User following)
        //{
            
        //    follower.Following.Add(follow);
        //    following.Following.Add(follow);
        //    await _container.UpsertItemAsync(follower);
        //    await _container.UpsertItemAsync(following);
        //}
    }
}
