using SM.Application.Abstractions;
using SM.Application.Authentication.Authorize.Models;
using SM.Domain.Shared;

namespace SM.Application.Authentication.Authorize;

public record AuthorizeCommand(string AccessToken) : ICommand<Result<AuthorizeResponse>>;
