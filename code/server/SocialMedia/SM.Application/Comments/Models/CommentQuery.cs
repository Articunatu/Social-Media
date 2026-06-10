using SM.Application.Shared.Models;
namespace SM.Application.Comments.Models;

public record CommentQuery : ProfilePostDto
{
    public Guid AuthorId { get; set; }
    public Guid ParentPostId { get; set; }
}
