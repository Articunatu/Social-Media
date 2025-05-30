using SM.Domain.Abstractions;
using SM.Domain.Users;

namespace SM.Domain.Photos;

public class Photo : Entity<Guid>
{
    public string PhotoUrl { get; set; } = string.Empty;

    public PhotoType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}
