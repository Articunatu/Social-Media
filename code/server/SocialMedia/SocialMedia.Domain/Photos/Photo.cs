using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Users;

namespace SocialMedia.Domain.Photos
{
    public class Photo : Entity<Guid>
    {
        // Property to store the URL of the photo (e.g., stored in a CDN)
        public string PhotoUrl { get; set; }

        // Optional: If you want to indicate the type of photo without using boolean flags,
        // you could consider using an enumeration.
        public PhotoType Type { get; set; }

        // Property to keep track of when the photo was uploaded
        public DateTime CreatedAt { get; set; }

        // Property to associate the photo with a user
        public Guid UserId { get; set; }

        // Optional navigation property for Entity Framework relationships
        public User User { get; set; }
    }

    // Optional enum to replace boolean flags
    public enum PhotoType
    {
        Profile,
        Background,
        Regular
    }
}
