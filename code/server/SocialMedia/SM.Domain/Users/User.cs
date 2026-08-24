using SM.Domain.Abstractions;
using SM.Domain.Authentication;
using SM.Domain.Users.Events;

namespace SM.Domain.Users;

public class User(Guid id, string tag, string firstName, string lastName, string email)
    : SoftDeletableEntity<Guid>(id), IFullName
{
    public string Tag { get; } = tag;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
    public string Email { get; private set; } = email;

    public virtual byte[] PasswordHash { get; private set; } = [];
    public virtual byte[] PasswordSalt { get; private set; } = [];
    public virtual Token? Token { get; set; }

    public static User Create(IUser request)
    {
        var user = new User(
            Guid.CreateVersion7(),
            request.Tag, 
            request.FirstName, 
            request.LastName,
            request.Email);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }

    public void SetLogin(byte[] passwordHash, byte[] passwordSalt)
    {
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

}
