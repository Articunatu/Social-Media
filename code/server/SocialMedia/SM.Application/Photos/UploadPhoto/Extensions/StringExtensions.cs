
namespace SM.Application.Photos.UploadPhoto.Extensions;

public static class StringExtensions
{
    public static string? GetContentType(this string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".bmp" => "image/bmp",
            ".tiff" or ".tif" => "image/tiff",
            ".avif" => "image/avif",
            _ => null
        };
    }
}
