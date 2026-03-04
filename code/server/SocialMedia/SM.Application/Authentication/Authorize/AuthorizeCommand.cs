using SM.Application.Abstractions;

namespace SM.Application.Authentication.Authorize;

using SM.Application.Authentication.Authorize.Models;
using SM.Domain.Shared;

public class AuthorizeCommand(string accessToken) : ICommand<Result<AuthorizeResponse>>
{
    public string AccessToken { get; set; } = accessToken;
}
