using SM.Application.Abstractions;
using SM.Application.Authentication.Authorize.Models;

namespace SM.Application.Authentication.Authorize;

public record AuthorizeCommand(string AccessToken) : ICommand<AuthorizeResponse>;
