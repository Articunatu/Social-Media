using SM.Application.Abstractions;
using SM.Application.Comments.Models;
using SM.Application.Shared.Models;

namespace SM.Application.Comments.GetComments;

public record GetCommentsQuery(Guid ParentPostId, PageFilter Filter) : IQuery<PagedFeed<CommentQuery>>;