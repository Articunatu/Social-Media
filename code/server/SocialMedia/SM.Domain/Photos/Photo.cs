using SM.Domain.Abstractions;

namespace SM.Domain.Photos;

public class Photo(Guid id) : Entity<Guid>(id)
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "image/jpg";
    public byte[] Data { get; set; } = [];
    public PhotoType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
}
