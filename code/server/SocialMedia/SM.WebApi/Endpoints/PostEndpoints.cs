using MediatR;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Posts.CreatePost;
using SM.Application.Posts.DeletePost;
using SM.Application.Posts.GetExploredPosts;
using SM.Application.Posts.GetFeed;
using SM.Application.Posts.GetPostById;
using SM.Application.Posts.GetProfilePosts;
using SM.Application.Shared.Models;
using SM.WebApi.Extensions;

namespace SM.WebApi.Endpoints;

public static class PostEndpoints
{
    public static RouteGroupBuilder MapPostEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/posts/");

        group.MapPost("create", CreatePost).RequireAuthorization();
        group.MapDelete("delete/{id}", DeletePost).RequireAuthorization();
        group.MapGet("get-feed", GetFeed).RequireAuthorization();
        group.MapGet("{id}", GetPostById);
        group.MapGet("get-posts-by-user/{userId}", GetProfilePosts);
        group.MapGet("get-explored-posts", GetExploredPosts);

        return group;
    }

    public static async Task<IResult> CreatePost([FromBody] CreatePostCommand command, ISender sender, HttpContext httpContext)
    {
        Guid userId = httpContext.GetLoggedInUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        command = command with { AuthorId = userId };
        var createdPost = await sender.Send(command);
        return TypedResults.Ok(createdPost);
    }

    public static async Task<IResult> DeletePost(Guid id, ISender sender, HttpContext httpContext)
    {
        Guid userId = httpContext.GetLoggedInUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        var deletedPost = await sender.Send(new DeletePostCommand(id));
        return TypedResults.Ok(deletedPost);
    }

    public static async Task<IResult> GetPostById(Guid id,
        ISender sender, HttpContext httpContext, int pageNumber)
    {
        var filter = new PageFilter { Index = pageNumber };
        var query = new GetPostByIdQuery(id, filter);
        var post = await sender.Send(query);

        return post.IsSuccess
            ? TypedResults.Ok(post.Value)
            : TypedResults.BadRequest(post.Error);
    }

    public static async Task<IResult> GetFeed(ISender sender, HttpContext httpContext, int pageNumber = 0)
    {
        var userId = httpContext.GetLoggedInUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        var filter = new PageFilter { Index = pageNumber };
        var query = new GetFeedQuery(userId, filter);
        var feed = await sender.Send(query);
        return TypedResults.Ok(feed);
    }

    public static async Task<IResult> GetProfilePosts(Guid userId, ISender sender, int pageNumber = 0)
    {
        var filter = new PageFilter { Index = pageNumber };
        var query = new GetProfilePostsQuery(userId, filter);
        var posts = await sender.Send(query);
        return TypedResults.Ok(posts);
    }

    public static async Task<IResult> GetExploredPosts(ISender sender)
    {
        var exploredPosts = await sender.Send(new GetExploredPostsQuery());
        return TypedResults.Ok(exploredPosts);
    }
}
