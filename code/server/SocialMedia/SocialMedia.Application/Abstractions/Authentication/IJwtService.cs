using SocialMedia.Domain.Shared;

namespace SocialMedia.Application.Abstractions.Authentication
{
    public interface IJwtService
    {
        Task<Result<string>> GetAccessTokenAsync(
            string email,
            string password,
            CancellationToken cancellationToken = default);
    }
}
