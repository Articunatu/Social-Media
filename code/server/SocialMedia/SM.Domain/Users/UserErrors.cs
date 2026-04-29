using SM.Domain.Shared;

namespace SM.Domain.Users;

public static class UserErrors
{
    public static readonly Error NotFound = new(
        "User.Found",
        "The user with the specified identifier was not found");

    public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "The provided credentials were invalid");

    public static readonly Error InvalidTag = new(
        "User.InvalidTag",
        "The provided tag is invalid");
}
