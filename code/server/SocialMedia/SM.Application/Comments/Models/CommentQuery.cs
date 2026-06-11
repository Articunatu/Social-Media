using SM.Application.Shared.Models;

namespace SM.Application.Comments.Models;

public record CommentQuery : ProfilePostDto
{
    public Guid AuthorId { get; set; }
    public ProfileInfo Author { get; set; } = default!;
    public Guid ParentPostId { get; set; }
    public IEnumerable<CommentQuery> Replies { get; set; } = [];
}
