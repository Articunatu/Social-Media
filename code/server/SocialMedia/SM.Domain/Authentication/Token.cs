using SM.Domain.Abstractions;
using SM.Domain.Users;

namespace SM.Domain.Authentication;

public class Token(Guid id) : Entity<Guid>(id)
{
    public string Text { get; private set; } = default!;
    public DateTimeOffset Created { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset Expires { get; private set; }
    public Guid UserId { get; private set; }
    public virtual User User { get; private set; } = default!;

    public static Token Create(Guid userId, string text, DateTimeOffset created, DateTimeOffset expires)
    {
        return new Token(Guid.CreateVersion7())
        {
            UserId = userId,
            Text = text,
            Created = created,
            Expires = expires
        };
    }

    public void Rotate(string text, DateTimeOffset created, DateTimeOffset expires)
    {
        Text = text;
        Created = created;
        Expires = expires;
    }
}