using SocialMedia.Application.Abstractions;
using SocialMedia.Application.Users.Queries.Application.Users.Queries.GetUserById;

namespace SocialMedia.Application.Users.Queries.GetUserById
{
    public sealed record GetUserByIdQuery(Guid userId) : IQuery<UserResponse>
    { }
}
