using Application.Abstraction;

namespace Application.Users.Queries.GetUserById
{
    public sealed record GetUserByIdQuery(Guid userId) : IQuery<UserResponse> 
    { }
}
