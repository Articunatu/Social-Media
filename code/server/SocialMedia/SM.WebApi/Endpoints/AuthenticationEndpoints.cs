using MediatR;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Authentication.Authorize;
using SM.Application.Authentication.ChangePassword;
using SM.Application.Authentication.Login;
using SM.Application.Authentication.Logout;
using SM.Application.Authentication.RefreshToken;
using SM.Application.Authentication.SignUp;
using SM.Domain.Authentication;
using SM.WebApi.Extensions;

namespace SM.WebApi.Endpoints;

public static class AuthenticationEndpoints
{
    public static RouteGroupBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/auth");

        group.MapPost("/signup", SignUp);
        group.MapPost("/login", Login);
        group.MapGet("/authorize", Authorize);
        group.MapPost("/logout", Logout).RequireAuthorization();
        group.MapPost("/refresh-token", RefreshToken);
        group.MapPost("/change-password", ChangePassword);

        return group;
    }

    public static async Task<IResult> SignUp([FromBody] SignUpCommand command, ISender sender)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    public static async Task<IResult> Login([FromBody] LoginCommand command,
        ISender sender,
        IHttpContextAccessor accessor)
    {
        var result = await sender.Send(command);

        if (result.IsSuccess && result.Value != null)
            SetRefreshToken(accessor, result.Value.RefreshToken);

        return result.ToActionResult();
    }

    public static async Task<IResult> Authorize(ISender sender, HttpContext httpContext)
    {
        var authHeader = httpContext.Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            return TypedResults.Unauthorized();

        var accessToken = authHeader.Substring("Bearer ".Length).Trim();

        var result = await sender.Send(new AuthorizeCommand(accessToken));
        if (!result.IsSuccess || result.Value is null)
            return TypedResults.Unauthorized();

        return TypedResults.Ok(result.Value);
    }

    public static async Task<IResult> Logout([FromBody] LogoutCommand command, ISender sender)
    {
        await sender.Send(command);
        return TypedResults.Ok("Logged out.");
    }

    public static async Task<IResult> RefreshToken([FromBody] RefreshTokenCommand command,
        ISender sender,
        HttpRequest request)
    {
        var refreshToken = request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return TypedResults.Unauthorized();

        var result = await sender.Send(new RefreshTokenCommand(refreshToken));

        return result.ToActionResult(token => TypedResults.Ok(token.AccessToken));
    }

    private static void SetRefreshToken(IHttpContextAccessor httpContextAccessor, Token newRefreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddDays(7)
        };
        httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", newRefreshToken.Text, cookieOptions);
    }

    private static async Task<IResult> ChangePassword([FromBody] ChangePasswordCommand command,
        ISender sender, HttpContext httpContext)
    {
        Guid userId = httpContext.User.GetUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        command = command with { UserId = userId };
        var result = await sender.Send(command);
        return TypedResults.Ok(result.Value);
    }
}
