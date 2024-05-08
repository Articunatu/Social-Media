using SocialMedia.Application.Abstractions;


namespace SocialMedia.Application.Users.RefreshToken
{
    internal class RefreshTokenCommand(string Email, string Password)
    : ICommand<object>;
}
