using SM.Domain.Photos;

namespace SM.Application.Photos;

public static class PhotoExtensions
{
    public static PhotoResponse MapToResponse(this Photo photo)
    {
        return new PhotoResponse
        {
            FileName = photo.FileName,
            ContentType = photo.ContentType,
            Base64Data = Convert.ToBase64String(photo.Data)
        };
    }
}
