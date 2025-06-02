using MediatR;
using SM.Application.Abstractions;
using SM.Domain.Authentication;

namespace SM.Application.Authentication.Login;

public record LoginCommand(string Tag, string Password) : IRequest<LoginResponse>;
