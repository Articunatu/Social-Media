using SM.Domain.Photos;

namespace SM.Application.Photos;

public class PhotoResponse
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "image/jpeg";
    public string Base64Data { get; set; } = string.Empty;

    public static PhotoResponse MapToResponse(Photo photo)
    {
        return new PhotoResponse
        {
            FileName = photo.FileName,
            ContentType = photo.ContentType,
            Base64Data = Convert.ToBase64String(photo.Data)
        };
    }
}
