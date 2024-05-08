using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Users.GetLoggedInId
{
    public sealed record GetLoggedInIdQuery() : IQuery<Guid> { }
}
