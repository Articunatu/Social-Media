using SM.Domain.Users;

namespace SM.Application.Profiles.Extensions;

public static class IFullNameExtensions
{
    public static string GetFullName(this IFullName user)
    {
        return user.FirstName + " " + user.LastName;
    }
}
