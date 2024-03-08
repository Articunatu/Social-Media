
namespace SocialMedia.Domain.Photos
{
    internal class Photo
    {
        public Guid Id { get; set; }
        public byte[] Base64 { get; set; }
        //size, check if vertical or horizontal
        public bool IsProfilePhoto { get; set; }
        public bool IsBackgroundPhoto { get; set; }
    }
}
