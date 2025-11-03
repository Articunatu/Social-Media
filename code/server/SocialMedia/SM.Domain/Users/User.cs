using SM.Domain.Abstractions;
using SM.Domain.Authentication;
using SM.Domain.Messages;
using SM.Domain.Photos;
using SM.Domain.Users.Events;
using SM.Domain.Users.ValueObjects;
using SM.Domain.Reactions;

namespace SM.Domain.Users;

public class User(Guid id, string tag, string firstName, string lastName)
    : Entity<Guid>(id), ISoftDeletable, IFullName
{
    public string Tag { get; set; } = tag;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = default!;

    public virtual bool IsDeleted { get; set; }
    public virtual DateTime? TimeOfDelete { get; set; }

    public virtual byte[] PasswordHash { get; set; } = [];
    public virtual byte[] PasswordSalt { get; set; } = [];
    public virtual Token? Token { get; set; }

    public virtual ICollection<Post> AuthoredPosts { get; set; } = [];
    public virtual ICollection<Comment> AuthoredComments { get; set; } = [];
    public virtual ICollection<Reaction> Reactions { get; set; } = [];
    public virtual ICollection<Photo> Photos { get; set; } = [];
    public virtual ICollection<User> Following { get; set; } = [];
    public virtual ICollection<User> Followers { get; set; } = [];

    public static User Create(IUser request)
    {
        var userId = Guid.CreateVersion7();

        var user = new User(userId, request.Tag, request.FirstName, request.LastName)
        {
            Email = request.Email
        };
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }

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

    public interface IUser
    {
        string Tag { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
    }
}
