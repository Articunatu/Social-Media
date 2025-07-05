using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SM.Application.Authentication.Login;
using SM.Application.Authentication.Logout;
using SM.Application.Authentication.RefreshToken;
using SM.Application.Authentication.SignUp;
using SM.Domain.Authentication;
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
        try
        {
            var login = await sender.Send(command);
            SetRefreshToken(accessor, login.Value.RefreshToken);
            return TypedResults.Ok(login.Value.AccessToken);
        }
        catch (UnauthorizedAccessException)
        {
            return TypedResults.Unauthorized();
        }
    }

    public static async Task<IResult> SignUp([FromBody] SignUpCommand command, ISender sender)
    {
        try
        {
            var createdUser = await sender.Send(command);

            return TypedResults.Ok(command);
        }
        catch (ValidationException)
        {
            return TypedResults.BadRequest("Validations failed for one or more fields when creating a new user");
        }
        catch (DbUpdateException dbEx)
        {
            if (dbEx.InnerException?.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) == true ||
                dbEx.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true)
                return TypedResults.Conflict("Email already exists");

            if (dbEx.InnerException?.Message.Contains("Insert", StringComparison.OrdinalIgnoreCase) == true ||
                dbEx.InnerException?.Message.Contains("string", StringComparison.OrdinalIgnoreCase) == true)
                return TypedResults.BadRequest("Validations failed when saving the created user to the database");

            return TypedResults.InternalServerError("A database error occurred");
        }
        catch (Exception)
        {
            return TypedResults.InternalServerError("An unknown server error occured");
        }
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
