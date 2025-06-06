using SM.Application.Abstractions;
using SM.Application.Shared.Models;

namespace SM.Application.Posts.GetPostById;

public record GetPostByIdQuery(Guid Id, ProfileInfo? Profile, ProfilePostDto? Post, Guid UserId) : IQuery<PostDetailsResponse>
{
    public ProfileInfo? Profile { get; set; } = Profile!;
    public ProfilePostDto? Post { get; set; } = Post!;
}
