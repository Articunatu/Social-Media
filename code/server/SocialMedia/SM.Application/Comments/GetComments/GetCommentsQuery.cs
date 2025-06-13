using SM.Application.Abstractions;
using SM.Application.Shared.Models;

namespace SM.Application.Comments.GetComments;

public record GetCommentsQuery(Guid Id, PageFilter Filter) : IQuery<PagedFeed<CommentResponse>>;