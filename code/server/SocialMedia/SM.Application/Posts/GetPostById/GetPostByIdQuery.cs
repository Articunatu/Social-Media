using SM.Application.Abstractions;
using SM.Application.Shared.Models;

namespace SM.Application.Posts.GetPostById;

public record GetPostByIdQuery(Guid Id, Guid UserId, PageFilter Filter) : IQuery<PostDetailsResponse>
{
    public ProfileInfo? Profile { get; set; }
    public ProfilePostDto? Post { get; set; }
}
