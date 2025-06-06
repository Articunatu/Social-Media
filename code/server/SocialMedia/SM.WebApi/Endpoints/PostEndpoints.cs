using MediatR;
using SM.Application.Posts.CreatePost;
using SM.Application.Posts.DeletePost;
using SM.Application.Posts.GetFeed;

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

    public static async Task<IResult> CreatePost(CreatePostCommand command, ISender sender)
    {
        var createdPost = await sender.Send(command);
        return TypedResults.Ok(createdPost);
    }

    public static async Task<IResult> DeletePost(DeletePostCommand command, ISender sender)
    {
        var deletedPost = await sender.Send(command);
        return TypedResults.Ok(deletedPost);
    }

    public static async Task<IResult> GetFeed(GetFeedQuery query, ISender sender)
    {
        var feed = await sender.Send(query);
        return TypedResults.Ok(feed);
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
