using MediatR;
using SM.Application.Authentication;
using SM.Domain.Authentication;
using System.Security.Claims;

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

    public static async Task<IResult> Login(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> Logout(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> RefreshToken(string tag, IHttpContextAccessor httpContextAccessor)
    {
        string refreshToken = httpContextAccessor.HttpContext!.Request.Cookies["refreshToken"]!;
        return TypedResults.Ok(refreshToken);
    }

    public static async Task<IResult> SignUp(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static Guid GetLoggedInUserId(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext is not null)
        {
            var userIdClaim = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdClaim, out Guid userId) ? userId : Guid.Empty;
        }
        return Guid.Empty;
    }

    public static string? NewRefreshToken(IJwtService jwtService, IConfiguration config, string tag)
    {
        string key = config.GetSection("AppSettings:Token").Value!;
        string token = jwtService.CreateToken(tag, key);
        var newRefreshToken = jwtService.GenerateRefreshToken();
        SetRefreshToken(new HttpContextAccessor(), newRefreshToken);
        return token;
    }

    public static void SetRefreshToken(IHttpContextAccessor httpContextAccessor, Token newRefreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", newRefreshToken.Text, cookieOptions);
    }
}
