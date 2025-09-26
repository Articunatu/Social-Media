
using SM.Application.Abstractions;
using SM.Application.Comments.Models;

namespace SM.Application.Comments.GetCommentById;

public record GetCommentByIdQuery(Guid Id) : IQuery<CommentQuery>;
