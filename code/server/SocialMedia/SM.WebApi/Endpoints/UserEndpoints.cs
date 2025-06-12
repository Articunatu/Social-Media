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
        var deletedAccount = await sender.Send(command);
        return TypedResults.Ok(deletedAccount);
    }

    public static async Task<IResult> Follow(FollowCommand command, ISender sender)
    {
        var followPair = await sender.Send(command);
        return TypedResults.Ok(followPair);
    }

    public static async Task<IResult> GetProfile(GetProfileQuery query, ISender sender)
    {
        var profile = await sender.Send(query);
        return TypedResults.Ok(profile);
    }

    public static async Task<IResult> SearchUsers(SearchUserQuery query, ISender sender)
    {
        var foundUsers = await sender.Send(query);
        return TypedResults.Ok(foundUsers);
    }

    public static async Task<IResult> Unfollow(UnfollowCommand command, ISender sender)
    {
        var unfollowPair = await sender.Send(command);
        return TypedResults.Ok(unfollowPair);
    }
}
