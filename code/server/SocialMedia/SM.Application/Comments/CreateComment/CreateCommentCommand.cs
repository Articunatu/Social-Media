
using SM.Application.Abstractions;

namespace SM.Application.Comments.CreateComment;

public record CreateCommentCommand(string Content, Guid AuthorId, Guid ParentPostId) : ICommand<CommentCommand>;