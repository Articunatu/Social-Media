using MediatR;
using SM.Application.Comments.CreateComment;
using SM.Application.Comments.DeleteComment;
using SM.Application.Comments.GetComments;

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

    public static async Task<IResult> CreateComment(CreateCommentCommand command, ISender sender)
    {
        var createdComment = await sender.Send(command);
        return TypedResults.Ok(createdComment);
    }

    public static async Task<IResult> DeleteComment(DeleteCommentCommand command, ISender sender)
    {
        var deletedComment = await sender.Send(command);
        return TypedResults.Ok(deletedComment);
    }

    public static async Task<IResult> GetComments(GetCommentsQuery query, ISender sender)
    {
        var comments = await sender.Send(query);
        return TypedResults.Ok(comments);
    }

    public static async Task<IResult> GetCommentById(ISender sender)
    {
        await sender.Send();
        return TypedResults.Ok();
    }
}
