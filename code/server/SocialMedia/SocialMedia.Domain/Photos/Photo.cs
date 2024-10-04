
using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Domain.Photos
{
    public class Photo : Entity<Guid>
    {
        public byte[] Base64 { get; set; }
        public bool IsProfilePhoto { get; set; }
        public bool IsBackgroundPhoto { get; set; }
    }
}