using MediatR;
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

        group.MapDelete("{userId}", DeleteAccount);
        group.MapPost("follow", Follow);
        group.MapGet("{userId}", GetProfile);
        group.MapPost("search", SearchUsers);
        group.MapDelete("unfollow", Unfollow);

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

    public static async Task<IResult> GetProfile(GetProfileQuery query, ISender sender)
    {
        var result = await sender.Send(query);
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> SearchUsers(SearchUserQuery query, ISender sender)
    {
        var result = await sender.Send(query);
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> Unfollow(UnfollowCommand command, ISender sender)
    {
        var result = await sender.Send(command);
        return TypedResults.Ok(result);
    }
}
