using SocialMedia.Domain.Users;

namespace SocialMedia.Application.Abstractions.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(
            User user,
            string password,
            CancellationToken cancellationToken = default);
    }
}
