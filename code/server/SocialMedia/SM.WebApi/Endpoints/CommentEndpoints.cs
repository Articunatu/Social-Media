using MediatR;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Comments.CreateComment;
using SM.Application.Comments.DeleteComment;
using SM.Application.Comments.GetCommentById;
using SM.Application.Comments.GetComments;
using SM.Application.Shared.Models;

namespace SM.WebApi.Endpoints;

public static class CommentEndpoints
{
    public static RouteGroupBuilder MapCommentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api");

        group.MapPost("/comments/", CreateComment);
        group.MapDelete("/comments/", DeleteComment);
        group.MapGet("/posts/{postId}/comments", GetComments);
        group.MapGet("/comments/{id}", GetCommentById);

        return group;
    }

    public static async Task<IResult> CreateComment([FromBody] CreateCommentCommand command, ISender sender)
    {
        var createdComment = await sender.Send(command);
        return TypedResults.Ok(createdComment);
    }

    public static async Task<IResult> DeleteComment([FromBody] DeleteCommentCommand command, ISender sender)
    {
        var deletedComment = await sender.Send(command);
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
