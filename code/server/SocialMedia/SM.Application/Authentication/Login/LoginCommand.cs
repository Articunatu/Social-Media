using SM.Application.Abstractions;

namespace SM.Application.Authentication.Login;

public record LoginCommand(string Tag, string Password) : ICommand<LoginResponse>;
