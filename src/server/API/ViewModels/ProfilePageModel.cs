
namespace API.ViewModels
{
    public class ProfilePageModel
    {
        public ProfileModel Profile;
        public int FollowerCount;
        public int FollowingCount;
        public int MutualCount;
        public IEnumerable<PostModel>? First10Posts;
        public ProfilePageModel(ProfileModel profile, IEnumerable<PostModel>? posts,
            int followerCount, int mutualCount, int followingCount)
        {
            Profile = profile;
            First10Posts = posts;
            MutualCount = mutualCount;
            FollowingCount = followingCount;
            FollowerCount = followerCount;
        }
    }
}
