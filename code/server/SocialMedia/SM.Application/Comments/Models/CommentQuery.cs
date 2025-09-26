using SM.Application.Shared.Models;
namespace SM.Application.Comments.Models;

public record CommentQuery : ProfilePostDto
{
    public Guid ParentPostId { get; set; }
}
