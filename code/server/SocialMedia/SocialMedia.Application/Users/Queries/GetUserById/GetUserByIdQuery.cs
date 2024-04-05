using SocialMedia.Application.Abstractions;
using SocialMedia.Application.Users.Queries.Application.Users.Queries.GetUserById;
using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Application.Users.Queries.GetUserById
{
    public sealed record GetUserByIdQuery(Guid Key) : IQuery<UserResponse>
    { }
}
