
namespace SM.Application.Photos;

public record PhotoResponse
{
    public Guid Id { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = "image/jpg";
    public string Base64Data { get; init; } = string.Empty;
}
