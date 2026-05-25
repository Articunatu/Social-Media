namespace SM.Domain.Users;

public interface IUser
{
    string Tag { get; init; }
    string FirstName { get; init; }
    string LastName { get; init; }
    string Email { get; init; }
}
