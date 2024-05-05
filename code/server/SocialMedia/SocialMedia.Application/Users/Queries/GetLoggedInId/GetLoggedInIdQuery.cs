using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Users.Queries.GetLoggedInId
{
    public sealed record GetLoggedInIdQuery() : IQuery<Guid> { }
}
