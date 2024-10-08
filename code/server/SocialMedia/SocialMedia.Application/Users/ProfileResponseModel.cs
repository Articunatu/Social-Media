using SocialMedia.Domain.Photos;
using SocialMedia.Domain.Users;

namespace SocialMedia.Application.Users
{
    public class ProfileResponseModel(Guid id, string tag, string fullname, Photo profilePhoto)
    {
        public Guid Id { get; set; } = id;
        public string Tag { get; set; } = tag;
        public string Fullname { get; set; } = fullname;
        public Photo ProfilePhoto { get; set; } = profilePhoto;
        public static ProfileResponseModel Map(User user, Photo photo)
        {
            return new ProfileResponseModel(user.Id, user.Tag, user.FirstName + " " + user.LastName, photo);
        }
    }
}
