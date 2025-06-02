using MediatR;

namespace SM.Application.Authentication.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<LoginResponse>;
