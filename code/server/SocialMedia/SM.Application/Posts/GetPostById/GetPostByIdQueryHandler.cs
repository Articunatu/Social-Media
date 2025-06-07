using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Authentication.SignUp;
using SM.Application.Database;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Posts.GetPostById;

internal class GetPostByIdQueryHandler(ApplicationDbContext context) : IQueryHandler<GetPostByIdQuery, PostDetailsResponse>
{
    public async Task<Result<PostDetailsResponse>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Profile is null)
        {
            //request.Profile = (await context.Users
            //    .Where(u => u.Id == request.UserId)
            //    .Select(u => new User(u.Id, u.Tag, u.FirstName, u.LastName))
            //    .FirstOrDefaultAsync()
            //    .Result).MapToSignUpResponse();
        }

        request.Post ??= await context.Posts
                .Where(p => p.AuthorId == request.UserId)
                .Select(p => new ProfilePostDto
                {
                    Content = p.Content,
                    TimeStamp = p.TimeStamp,
                    CommentsCount = p.Replies != null ? p.Replies.Count() : 0,
                    ReactionCounts = p.Reactions != null
                        ? p.Reactions
                            .GroupBy(r => r.Type)
                            .Select(rt => new ReactionCount(rt.Key, rt.Count()))
                        : new List<ReactionCount>()
                }).FirstOrDefaultAsync();


    }
}
