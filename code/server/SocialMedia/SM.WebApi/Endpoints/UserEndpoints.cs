using MediatR;
using SM.Application.Users.DeleteAccount;
using SM.Application.Users.Follow;

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
        group.MapPost("/users/{userId}/follow", Unfollow);

        return group;
    }

    public static async Task<IResult> DeleteAccount(DeleteAccountCommand command, ISender sender)
    {
        var result = await sender.Send(command);
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> Follow(FollowCommand command, ISender sender)
    {
        var result = await sender.Send(command);
        return TypedResults.Ok(result);
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
