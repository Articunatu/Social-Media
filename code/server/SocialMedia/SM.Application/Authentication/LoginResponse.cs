using SM.Domain.Authentication;

namespace SM.Application.Authentication;

public record LoginResponse(string AccessToken, Token RefreshToken);
