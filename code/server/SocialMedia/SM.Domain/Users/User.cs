using SM.Domain.Abstractions;
using SM.Domain.Authentication;
using SM.Domain.Messages;
using SM.Domain.Photos;
using SM.Domain.Users.Events;

namespace SM.Domain.Users;

public class User(Guid id, string tag, string firstName, string lastName)
    : Entity<Guid>(id), ISoftDeletable, IFullName
{
    public string Tag { get; set; } = tag;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = default!;

    public bool IsDeleted { get; set; }
    public DateTime? TimeOfDelete { get; set; }

    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public Token? Token { get; set; }

    public ICollection<Message> AuthoredMessages { get; set; } = [];
    public ICollection<Post> ReactedPosts { get; set; } = [];
    //public ICollection<DirectMessage>? DirectMessages { get; set; }
    public ICollection<Photo> Photos { get; set; } = [];
    public Guid? ProfilePhotoId { get; set; }
    public virtual Photo ProfilePhoto { get; set; } = default!;
    public virtual ICollection<User> Following { get; set; } = [];
    public virtual ICollection<User> Followers { get; set; } = [];

    public static User Create(string tag, string firstName, string lastName, string email)
    {
        var userId = Guid.CreateVersion7();

        var user = new User(userId, tag, firstName, lastName)
        {
            Email = email
        };
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
