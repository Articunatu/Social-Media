using SM.Application.Abstractions;
using SM.Application.Authentication.Authorize.Models;
using SM.Domain.Shared;

namespace SM.Application.Authentication.Authorize;

public class AuthorizeCommand(string accessToken) : ICommand<Result<AuthorizeResponse>>
{
    public string AccessToken { get; set; } = accessToken;
}
