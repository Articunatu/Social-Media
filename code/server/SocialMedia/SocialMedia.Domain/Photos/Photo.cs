
namespace SocialMedia.Domain.Photos
{
    public sealed class Photo
    {
        public Guid Id { get; set; }
        public byte[] Base64 { get; set; }
        public bool IsProfilePhoto { get; set; }
        public bool IsBackgroundPhoto { get; set; }

        public bool IsVertical()
        {
            // Check if the photo is vertical
            // You may need to decode the Base64 string and inspect its dimensions
            // This example assumes that the photo is vertical if its height is greater than its width
            // Replace this logic with your own based on how you access the dimensions of the photo
            // For simplicity, this example assumes Base64 string represents an image format with metadata
            // showing width and height separated by "x"

            string base64String = Convert.ToBase64String(Base64);
            // Sample format "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAAAAAA...."
            string[] parts = base64String.Split(',');
            if (parts.Length < 2)
            {
                throw new ArgumentException("Invalid Base64 string. Unable to determine photo dimensions.");
            }

            // Extracting dimensions
            string[] dimensions = parts[0].Split(';')[1].Split(' ')[2].Split('x');
            int width = int.Parse(dimensions[0]);
            int height = int.Parse(dimensions[1]);

            // Determine if it's vertical or horizontal
            return height > width;
        }
    }
}
