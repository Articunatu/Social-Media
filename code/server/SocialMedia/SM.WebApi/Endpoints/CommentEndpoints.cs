using MediatR;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Comments.CreateComment;
using SM.Application.Comments.DeleteComment;
using SM.Application.Comments.GetCommentById;
using SM.Application.Comments.GetComments;
using SM.Application.Shared.Models;
using SM.WebApi.Extensions;

namespace SM.WebApi.Endpoints;

public static class CommentEndpoints
{
    public static RouteGroupBuilder MapCommentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/comments/");

        group.MapPost("create", CreateComment).RequireAuthorization();
        group.MapDelete("delete/{id}", DeleteComment).RequireAuthorization();
        group.MapGet("get-by-post-id/{postId}", GetComments);
        group.MapGet("{id}", GetCommentById);

        return group;
    }

    public static async Task<IResult> CreateComment([FromBody] CreateCommentCommand command, ISender sender, HttpContext httpContext)
    {
        Guid userId = httpContext.GetLoggedInUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        command = command with { AuthorId = userId };

        var createdComment = await sender.Send(command);
        return TypedResults.Ok(createdComment);
    }

    public static async Task<IResult> DeleteComment(Guid id, ISender sender, HttpContext httpContext)
    {
        Guid userId = httpContext.GetLoggedInUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        var deletedComment = await sender.Send(new DeleteCommentCommand(id));
        return TypedResults.Ok(deletedComment);
    }

    public static async Task<IResult> GetComments(Guid postId, [AsParameters] PageFilter filter, ISender sender)
    {
        var query = new GetCommentsQuery(postId, filter);
        var comments = await sender.Send(query);
        return TypedResults.Ok(comments);
    }

    public static async Task<IResult> GetCommentById(Guid id, ISender sender)
    {
        var query = new GetCommentByIdQuery(id);
        var comment = await sender.Send(query);
        return TypedResults.Ok(comment);
    }
}
