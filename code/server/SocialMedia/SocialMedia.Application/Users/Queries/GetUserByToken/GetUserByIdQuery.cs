using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Application.Users.Queries.GetUserByToken
{
    public sealed record GetUserByTokenQuery(object Key, KeyTypes KType) : IQuery<UserResponse> { }
}
