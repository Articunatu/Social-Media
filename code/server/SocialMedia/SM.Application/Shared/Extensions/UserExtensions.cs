using SM.Application.Shared.Models;
using SM.Application.Users;
using SM.Domain.Photos;
using SM.Domain.Users;
using SM.Domain.Users.Extensions;

namespace SM.Application.Shared.Extensions;

public static class UserExtensions
{
    public static ProfileInfo MapToProfile(this User user)
    {
        return new ProfileInfo(user.Id, user.Tag, user.GetFullName(), user.GetProfilePhoto());
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

    public static Photo? GetProfilePhoto(this User user)
    {
        return user.Photos.Where(p => p.Type == PhotoType.Profile)
                .MaxBy(p => p.CreatedAt);
    }

}
