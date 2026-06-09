using MediatR;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Users.DeleteAccount;
using SM.Application.Users.Follow;
using SM.Application.Users.GetProfile;
using SM.Application.Users.SearchUsers;
using SM.Application.Users.Unfollow;
using SM.WebApi.Extensions;

namespace SM.WebApi.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users/");

        group.MapDelete("delete", DeleteAccount).RequireAuthorization();
        group.MapPost("follow", Follow).RequireAuthorization();
        group.MapGet("{userId}", GetProfile).RequireAuthorization();
        group.MapPost("search", SearchUsers);
        group.MapDelete("unfollow", Unfollow).RequireAuthorization();

        return group;
    }

    public static async Task<IResult> DeleteAccount(
        [FromBody] DeleteAccountCommand command,
        ISender sender, HttpContext httpContext)
    {
        var userId = httpContext.GetLoggedInUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        var deletedAccount = await sender.Send(command);
        return TypedResults.Ok(deletedAccount);
    }

    public static async Task<IResult> Follow(
        [FromBody] FollowCommand command,
        ISender sender, HttpContext httpContext)
    {
        var userId = httpContext.GetLoggedInUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        command = command with { FollowerId = userId };

        var followPair = await sender.Send(command);
        return followPair.ToActionResult();
    }

    public static async Task<IResult> GetProfile(
        Guid userId,
        ISender sender,
        HttpContext httpContext)
    {
        var viewerId = httpContext.GetLoggedInUserId();
        var query = new GetProfileQuery(userId, viewerId);
        var profile = await sender.Send(query);
        return profile.ToActionResult();
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
        ISender sender, HttpContext httpContext)
    {
        var userId = httpContext.GetLoggedInUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        command = command with { FollowerId = userId };

        var unfollowPair = await sender.Send(command);
        return unfollowPair.ToActionResult();
    }
}
