namespace SM.WebApi.Endpoints;

public static class PhotoEndpoints
{
    public static RouteGroupBuilder MapPhotoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/comments/");

        group.MapPost("upload", UploadPhoto);
        group.MapGet("get-by-post-id/{postId}", GetComments);
        group.MapGet("{id}", GetCommentById);

        return group;
    }
}
