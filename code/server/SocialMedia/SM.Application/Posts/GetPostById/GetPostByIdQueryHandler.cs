using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;
using SM.Domain.Users;

namespace SM.Application.Posts.GetPostById;

internal class GetPostByIdQueryHandler(ApplicationDbContext context) : IQueryHandler<GetPostByIdQuery, PostDetailsResponse>
{
    public async Task<Result<PostDetailsResponse>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Profile is null)
        {
            var user = await context.Users
                .Where(u => u.Id == request.UserId)
                .Select(u => new User(u.Id, u.Tag, u.FirstName, u.LastName))
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result.Failure<PostDetailsResponse>(new Error("User.NotFound"));

            request.Profile = user.MapToProfile();
        }                

        request.Post ??= await context.Posts
                .Where(p => p.Id == request.Id)
                .Select(p => new ProfilePostDto
                {
                    Content = p.Content,
                    TimeStamp = p.TimeStamp,
                    CommentsCount = p.Comments != null ? p.Comments.Count() : 0,
                    ReactionCounts = p.Reactions != null
                        ? p.Reactions
                            .GroupBy(r => r.Type)
                            .Select(rt => new ReactionCount(rt.Key, rt.Count()))
                        : new List<ReactionCount>()
                }).FirstOrDefaultAsync();

        if (request.Post is null)
            return Result.Failure<PostDetailsResponse>(new Error("Post.NotFound"));

        var comments = context.Comments
            .Where(c => c.ParentPostId == request.Id)
            .Select(p => new ProfilePostDto
            {
                Content = p.Content,
                TimeStamp = p.TimeStamp,
                CommentsCount = p.Comments != null ? p.Comments.Count() : 0,
                ReactionCounts = p.Reactions != null
                        ? p.Reactions
                            .GroupBy(r => r.Type)
                            .Select(rt => new ReactionCount(rt.Key, rt.Count()))
                        : new List<ReactionCount>()
            }).AsQueryable();

        var pagedComments = await comments.ToPagedFeed(request.Filter);

        return Result.Success(new PostDetailsResponse(request.Profile, request.Post, pagedComments));
    }
}
