using MediatR;

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

    public static async Task<IResult> RefreshToken(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> SignUp(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }
}
