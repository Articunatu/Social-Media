using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;

namespace SocialMedia.Application.Users.GetUserById
{
    internal sealed class GetUserByIdQueryHandler(IUserNoSqlRepository userRepository)
                : IQueryHandler<GetUserByIdQuery, UserResponse>
    {
        readonly IUserNoSqlRepository _userRepository = userRepository;

        public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetSingle(x => x.Id == request.Id);

            if (user is null)
                return Result.Failure<UserResponse>(new Error("User.NotFound"));

            var response = new UserResponse(user.Id, user.Tag, user.GetFullName());
            return Result.Success(response);
        }
    }
}
