using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Application.Users.GetUserById
{
    public sealed record GetUserByIdQuery(object Key, KeyEnum KType) : IQuery<UserResponse> { }
}
