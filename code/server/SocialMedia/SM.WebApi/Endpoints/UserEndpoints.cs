using MediatR;

namespace SM.WebApi.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users");

        group.MapDelete("/{userId}", DeleteAccount);
        group.MapPost("/{userId}/follow", Follow);
        group.MapGet("/{userId}", GetProfile);
        group.MapGet("/{userId}/reactions", GetReactedPostsByUserId);
        group.MapDelete("/users/{userId}/follow", Unfollow);

        return group;
    }

    public static async Task<IResult> DeleteAccount(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> Follow(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> GetProfile(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> GetReactedPostsByUserId(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> Unfollow(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }
}
