namespace SM.Application.Authentication.Authorize.Models;

public record AuthorizeResponse(bool IsAuthorized, string? UserId, string? UserName);
