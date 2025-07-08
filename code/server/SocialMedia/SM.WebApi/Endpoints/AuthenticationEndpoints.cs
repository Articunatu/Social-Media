using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SM.Application.Authentication.Login;
using SM.Application.Authentication.Logout;
using SM.Application.Authentication.RefreshToken;
using SM.Application.Authentication.SignUp;
using SM.Domain.Authentication;
using SM.WebApi.Extensions;
using ValidationException = FluentValidation.ValidationException;

namespace SM.WebApi.Endpoints;

public static class AuthenticationEndpoints
{
    public static RouteGroupBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/auth");

        group.MapPost("/login", Login);
        group.MapPost("/logout", Logout);
        group.MapPost("/refresh-token", RefreshToken);
        group.MapPost("/signup", SignUp);

        return group;
    }
    public static async Task<IResult> Login([FromBody] LoginCommand command, ISender sender, IHttpContextAccessor accessor)
    {
        var result = await sender.Send(command);

        if (result.IsSuccess)
            SetRefreshToken(accessor, result.Value.RefreshToken);

        return result.ToActionResult();
    }


    public static async Task<IResult> SignUp([FromBody] SignUpCommand command, ISender sender)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }


    public static async Task<IResult> RefreshToken([FromBody] RefreshTokenCommand command, ISender sender, HttpRequest httpRequest)
    {
        var refreshToken = httpRequest.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return TypedResults.Unauthorized();

        var result = await sender.Send(new RefreshTokenCommand(refreshToken));
        return TypedResults.Ok(result.AccessToken);
    }

    public static async Task<IResult> Logout([FromBody] LogoutCommand command, ISender sender)
    {
        await sender.Send(command);
        return TypedResults.Ok("Logged out.");
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
}
