
using SM.Application.Abstractions;
using SM.Application.Comments.Models;

namespace SM.Application.Comments.CreateComment;

public record CreateCommentCommand(string Content, Guid AuthorId, Guid ParentPostId) : ICommand<CommentCommand>;