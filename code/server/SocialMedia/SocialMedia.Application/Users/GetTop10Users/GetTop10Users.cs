using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Users.GetTop10Users
{
    public sealed record GetTop10UsersQuery(int PageNumber) : IQuery<IEnumerable<UserResponse>>
    { }
}
