using Application.Abstraction;
using Domain.Shared;
using Infrastructure.Repositories;

namespace Application.Users.Queries.GetUserById
{
    internal sealed class GetUserByIdQueryHandler
        : IQueryHandler<GetUserByIdQuery, UserResponse>
    {
        readonly UserRepository _userRepository;

        public GetUserByIdQueryHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(
               request.userId,
               cancellationToken);

            if (user is null)
            {
                return Result.Failure<UserResponse>(new Error(
                    "User.NotFound"));
            }
            string userFullname = user.FirstName + " " + user.LastName;
            var response = new UserResponse(user.Id, user.Tag, userFullname);

            return Result<UserResponse>.Success(response);
        }

    }
}
