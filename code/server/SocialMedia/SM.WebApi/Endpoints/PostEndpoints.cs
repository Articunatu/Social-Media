using MediatR;

namespace SM.WebApi.Endpoints;

public static class PostEndpoints
{
    public static RouteGroupBuilder MapPostEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/posts");

        group.MapPost("/", CreatePost);
        group.MapDelete("/{id}", DeletePost);
        group.MapGet("/", GetFeed);
        group.MapGet("/{id}", GetPostById);
        group.MapGet("profile/{userId}", GetProfilePosts);

        return group;
    }

    public static async Task<IResult> CreatePost(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> DeletePost(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> GetFeed(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> GetPostById(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> GetProfilePosts(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }
}
