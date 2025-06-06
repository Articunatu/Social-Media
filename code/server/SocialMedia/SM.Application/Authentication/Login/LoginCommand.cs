using MediatR;

namespace SM.Application.Authentication.Login;

public record LoginCommand(string Tag, string Password) : IRequest<LoginResponse>;
