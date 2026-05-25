namespace SM.Domain.Users;

public record UserDto(string Tag, 
    string FirstName, 
    string LastName, 
    string Email) : IUser;
