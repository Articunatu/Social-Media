using SM.Domain.Abstractions;
using SM.Domain.Users;

namespace SM.Domain.Photos;

public class Photo(Guid id) : Entity<Guid>(id)
{
    public string PhotoUrl { get; set; } = string.Empty;

    public PhotoType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
}
