
using SocialMedia.Domain.Users;

namespace SocialMedia.Application.Users.Queries.GetUserByToken
{
     public sealed record UserResponse(Guid Id, string Tag, RefreshToken Token);
}
