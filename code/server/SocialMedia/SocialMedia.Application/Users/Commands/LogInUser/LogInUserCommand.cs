using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Application.Users.Commands.LogInUser
{
    public sealed record LogInUserCommand(string Email, string Password, KeyTypes KeyType)
    : ICommand<AccessTokenResponse>;
}
