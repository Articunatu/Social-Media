using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;
using System.Security.Principal;

namespace SocialMedia.Application.Users.Queries.GetUserByToken
{
    internal sealed class GetUserByTokenQueryHandler(IUserReadRepository userRepository)
                : IQueryHandler<GetUserByTokenQuery, UserResponse>
    {
        readonly IUserReadRepository _userRepository = userRepository;

        public async Task<Result<UserResponse>> Handle(GetUserByTokenQuery request, CancellationToken cancellationToken)
        {
            var query = $"SELECT c.id, c.tag c.token FROM c WHERE c.token = @partitionKey";
            var user = await _userRepository.GetSingle<User>(request.Key, query);
            if (user is null || user.Token.Expires < DateTime.Now)
                return Result.Failure<UserResponse>(new Error("Token expired or invalid."));
            var response = new UserResponse(user.Id, user.Tag, user.Token);
            return Result.Success(response);
        }

    }
}
