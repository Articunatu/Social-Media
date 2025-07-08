using MediatR;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Posts.CreatePost;
using SM.Application.Posts.DeletePost;
using SM.Application.Posts.GetFeed;
using SM.Application.Posts.GetPostById;
using SM.Application.Posts.GetProfilePosts;
using SM.Application.Shared.Models;

namespace SM.WebApi.Endpoints;

public static class PostEndpoints
{
    public static RouteGroupBuilder MapPostEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/posts/");

        group.MapPost("create", CreatePost);
        group.MapDelete("delete/{id}", DeletePost);
        group.MapGet("get-feed", GetFeed);
        group.MapGet("{id}", GetPostById);
        group.MapGet("get-posts-by-user/{userId}", GetProfilePosts);

        return group;
    }

    public static async Task<IResult> CreatePost([FromBody] CreatePostCommand command, ISender sender)
    {
        var createdPost = await sender.Send(command);
        return TypedResults.Ok(createdPost);
    }

    public static async Task<IResult> DeletePost(Guid id, ISender sender)
    {
        var deletedPost = await sender.Send(new DeletePostCommand(id));
        return TypedResults.Ok(deletedPost);
    }

    public static async Task<IResult> GetPostById(
        Guid id,
        int pageNumber,
        ISender sender,
        HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.FindFirst("sub")?.Value;
        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return TypedResults.Unauthorized();

        var filter = new PageFilter { Index = pageNumber };
        var query = new GetPostByIdQuery(id, userId, filter);
        var result = await sender.Send(query);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.BadRequest(result.Error);
    }

    public static async Task<IResult> GetFeed(
        int pageNumber,
        ISender sender,
        HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.FindFirst("sub")?.Value;
        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return TypedResults.Unauthorized();

        var filter = new PageFilter { Index = pageNumber };
        var query = new GetFeedQuery(userId, filter);
        var feed = await sender.Send(query);
        return TypedResults.Ok(feed);
    }

    public static async Task<IResult> GetProfilePosts(Guid userId, int pageNumber, ISender sender)
    {
        var filter = new PageFilter { Index = pageNumber };
        var query = new GetProfilePostsQuery(userId, filter);
        var posts = await sender.Send(query);
        return TypedResults.Ok(posts);
    }
}