using SM.Application.Photos;
using SM.Application.Shared.Models;
using SM.Application.Users;
using SM.Domain.Users;
using SM.Domain.Users.Extensions;

namespace SM.Application.Shared.Extensions;

public static class UserExtensions
{
    public static ProfileInfo MapToProfile(this User user, PhotoResponse? profilePhoto = null)
    {
        return new ProfileInfo(user.Id, user.Tag, user.GetFullName(), profilePhoto);
    }

    public static UserCommandResponse MapToCommandResponse(this User user)
    {
        return new UserCommandResponse
        {
            Id = user.Id,
            Tag = user.Tag,
            FullName = user.GetFullName(),
            Email = user.Email
        };
    }

}
