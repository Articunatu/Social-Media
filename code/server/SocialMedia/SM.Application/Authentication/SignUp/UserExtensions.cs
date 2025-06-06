using SM.Domain.Users;
using SM.Domain.Users.Extensions;

namespace SM.Application.Authentication.SignUp;

public static class UserExtensions
{
    public static SignUpResponse MapToSignUpResponse(this User user)
    {
        return new SignUpResponse(user.Id, user.Tag, user.GetFullName(), user.Email);
    }
}
