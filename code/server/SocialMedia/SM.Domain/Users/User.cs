using SM.Domain.Abstractions;
using SM.Domain.Messages;
using SM.Domain.Messages.DirectMessages;
using SM.Domain.Photos;
using SM.Domain.Users.Authentication;
using SM.Domain.Users.Events;

namespace SM.Domain.Users;

public class User(Guid id, string tag, string firstName, string lastName, string email)
    : Entity<Guid>(id), ISoftDeletable, IFullName
{
    public string Tag { get; set; } = tag;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;

    public bool IsDeleted { get; set; }
    public DateTime? TimeOfDelete { get; set; }

    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public Token? Token { get; set; }

    public ICollection<Post>? Posts { get; set; }
    public ICollection<DirectMessage> DirectMessages { get; set; } = [];
    public ICollection<Photo>? Photos { get; set; }
    public Photo? PhotoPhoto { get; set; };
    public ICollection<Guid> FollowingIds { get; set; } = [];
    public ICollection<Guid> FollowerIds { get; set; } = [];

    public static User Create(string tag, string firstName, string lastName, string email)
    {
        var userId = Guid.CreateVersion7();

        var user = new User(userId, tag, firstName, lastName, email);
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }

    public void SetLogin(byte[] passwordHash, byte[] passwordSalt)
    {
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public static void SetLoginForUsers(IEnumerable<User> users, byte[] passwordHash, byte[] passwordSalt)
    {
        foreach (var user in users)
            user.SetLogin(passwordHash, passwordSalt);
    }
}
