using MediatR;
using SM.Application.Comments.CreateComment;

namespace SM.WebApi.Endpoints;

public static class CommentEndpoints
{
    public static RouteGroupBuilder MapCommentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api");

        group.MapPost("/posts/{postId}/comments", CreateComment);
        group.MapDelete("/comments/{id}", DeleteComment);
        group.MapGet("/posts/{postId}/comments", GetReplies);
        group.MapGet("/comments/{id}", GetReplyById);

        return group;
    }

    public static async Task<IResult> CreateComment(CreateCommentCommand command, ISender sender)
    {
        var createdComment = await sender.Send(command);
        return TypedResults.Ok(createdComment);
    }

    public static async Task<IResult> DeleteComment(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> GetReplies(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> GetReplyById(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }
}
