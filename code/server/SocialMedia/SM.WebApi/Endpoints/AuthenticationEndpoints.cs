using MediatR;
using SM.Application.Authentication.Login;
using SM.Application.Authentication.Logout;
using SM.Application.Authentication.RefreshToken;
using SM.Application.Authentication.SignUp;
using SM.Domain.Authentication;

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

    public static async Task<IResult> Login(LoginCommand command, ISender sender, IHttpContextAccessor accessor)
    {
        try
        {
            var result = await sender.Send(command);
            SetRefreshToken(accessor, result.RefreshToken);
            return TypedResults.Ok(result.AccessToken);
        }
        catch (UnauthorizedAccessException)
        {
            return TypedResults.Unauthorized();
        }
    }

    public static async Task<IResult> SignUp(SignUpCommand command, ISender sender)
    {
        await sender.Send(command);
        return TypedResults.Ok("User registered.");
    }

    public static async Task<IResult> RefreshToken(RefreshTokenCommand command, ISender sender, HttpRequest httpRequest)
    {
        var refreshToken = httpRequest.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return TypedResults.Unauthorized();

        var result = await sender.Send(new RefreshTokenCommand(refreshToken));
        return TypedResults.Ok(result.AccessToken);
    }

    public static async Task<IResult> Logout(LogoutCommand command, ISender sender)
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
