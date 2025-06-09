using SM.Application.Shared.Models;
using SM.Domain.Users;
using SM.Domain.Users.Extensions;

namespace SM.Application.Shared.Extensions;

public static class UserExtensions
{
    public static ProfileInfo MapToProfile(this User user)
    {
        return new ProfileInfo(user.Id, user.Tag, user.GetFullName(), user.ProfilePhoto);
    }
}
