
namespace API.ViewModels
{
    public record ProfilePageModel
    {
        public ProfileModel Profile;
        public IEnumerable<PostModel>? Posts;
        public ProfilePageModel(ProfileModel profile, IEnumerable<PostModel>? posts)
        {
            Profile = profile;
            Posts = posts;
        }
    }
}
