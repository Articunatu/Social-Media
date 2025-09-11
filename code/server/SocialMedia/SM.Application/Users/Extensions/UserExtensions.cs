using SM.Domain.Users;
using SM.Domain.Users.Extensions;

namespace SM.Application.Users.Extensions;

public static class UserExtensions
{
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
