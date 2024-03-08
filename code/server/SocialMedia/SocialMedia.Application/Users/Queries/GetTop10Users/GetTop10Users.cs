using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Users.Queries.GetTop10Users
{
    public sealed record GetTop10UsersQuery(Guid UserId) : IQuery<IEnumerable<UserResponse>>
    { }
}
