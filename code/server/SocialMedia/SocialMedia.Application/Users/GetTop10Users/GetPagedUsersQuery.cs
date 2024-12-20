using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Users.GetTop10Users
{
    public sealed record GetPagedUsersQuery(int PageNumber) : IQuery<IEnumerable<PagedUsersResponse>>
    { }
}
