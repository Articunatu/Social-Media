using MediatR;

namespace SM.Application.Authentication.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<Unit>;
