using SM.Application.Abstractions;
using SM.Application.Shared.Models;

namespace SM.Application.Posts.GetPostById;

public record PostDetailsResponse(ProfileInfo ProfileMain, ProfilePostDto Post, PagedFeed<ProfilePostDto> Comments);
