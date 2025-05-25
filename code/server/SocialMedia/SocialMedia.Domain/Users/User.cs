using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Messages;
using SocialMedia.Domain.Messages.DirectMessages;
using SocialMedia.Domain.Photos;
using SocialMedia.Domain.Users.Authentication;
using SocialMedia.Domain.Users.Events;

namespace SocialMedia.Domain.Users;

public class User(Guid id, string tag, string firstName, string lastName, string email) 
    : Entity<Guid>(id), ISoftDeletable
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
    public ICollection<Post> Posts { get; set; } = [];
    public ICollection<DirectMessage> DirectMessages { get; set; } = [];
    public ICollection<Photo> Photos { get; set; } = [];
    public ICollection<Guid> FollowingIds { get; set; } = [];
    public ICollection<Guid> FollowerIds { get; set; } = [];
    public string FullName => FirstName + " " + LastName;

    public static User Create(string tag, string firstName, string lastName, string email)
    {
        var userId = Guid.NewGuid();

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

    public string GetFullName()
    {
        return FirstName + " " + LastName;
    }
}
