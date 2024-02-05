
namespace SocialMedia.Application.Users.Queries
{

    namespace Application.Users.Queries.GetUserById
    {
        public sealed record UserResponse(Guid Id, string Tag, string FullName);
    }
}
