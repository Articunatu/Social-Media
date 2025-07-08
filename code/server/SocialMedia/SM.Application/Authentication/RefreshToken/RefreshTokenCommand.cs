using SM.Application.Abstractions;

namespace SM.Application.Authentication.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand<LoginResponse>;
