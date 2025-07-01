
using SM.Application.Abstractions;

namespace SM.Application.Comments.GetCommentById;

public record GetCommentByIdQuery(Guid Id) : IQuery<CommentQuery>;
