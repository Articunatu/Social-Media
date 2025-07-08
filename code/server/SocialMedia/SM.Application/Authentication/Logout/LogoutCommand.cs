using MediatR;
using SM.Application.Abstractions;

namespace SM.Application.Authentication.Logout;

public record LogoutCommand(string RefreshToken) : ICommand<Unit>;
