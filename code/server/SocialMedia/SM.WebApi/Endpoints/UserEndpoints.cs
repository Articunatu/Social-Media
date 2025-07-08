using MediatR;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Users.DeleteAccount;
using SM.Application.Users.Follow;
using SM.Application.Users.GetProfile;
using SM.Application.Users.SearchUsers;
using SM.Application.Users.Unfollow;

namespace SM.WebApi.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users/");

        group.MapDelete("delete", DeleteAccount);
        group.MapPost("follow", Follow);
        group.MapGet("{userId}", GetProfile);
        group.MapPost("search", SearchUsers);
        group.MapDelete("unfollow", Unfollow);

        return group;
    }

    public static async Task<IResult> DeleteAccount(
        [FromBody] DeleteAccountCommand command,
        ISender sender)
    {
        var deletedAccount = await sender.Send(command);
        return TypedResults.Ok(deletedAccount);
    }

    public static async Task<IResult> Follow(
        [FromBody] FollowCommand command,
        ISender sender)
    {
        var followPair = await sender.Send(command);
        return TypedResults.Ok(followPair);
    }

    public static async Task<IResult> GetProfile(
        Guid userId,
        ISender sender)
    {
        var query = new GetProfileQuery(userId);
        var profile = await sender.Send(query);
        return TypedResults.Ok(profile);
    }

    public static async Task<IResult> SearchUsers(
        [FromBody] SearchUserQuery query,
        ISender sender)
    {
        var foundUsers = await sender.Send(query);
        return TypedResults.Ok(foundUsers);
    }

    public static async Task<IResult> Unfollow(
        [FromBody] UnfollowCommand command,
        ISender sender)
    {
        var unfollowPair = await sender.Send(command);
        return TypedResults.Ok(unfollowPair);
    }
}
