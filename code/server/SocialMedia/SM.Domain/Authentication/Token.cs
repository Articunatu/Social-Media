using SM.Domain.Abstractions;
using SM.Domain.Users;

namespace SM.Domain.Authentication;

public class Token(Guid id) : Entity<Guid>(id)
{
    public string Text { get; set; } = default!;
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset Expires { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
}