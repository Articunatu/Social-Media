using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;
using SocialMedia.Domain.Users.Authentication;
using SocialMedia.Domain.Users.ValueObjects;

namespace SocialMedia.Application.Users.AddUser
{
    internal sealed class AddUserCommandHandler : ICommandHandler<AddUserCommand, Guid>
    {
        readonly IUserRelationalRepository _userRepository;
        readonly IUnitOfWork _unitOfWork;
        readonly IAuthenticationService _authentication;

        public AddUserCommandHandler(IUserRelationalRepository userRepository, IUnitOfWork unitOfWork, IAuthenticationService authentication)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _authentication = authentication;
        }

        public async Task<Result<Guid>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var users = User.Create(
                request.Tag,
                request.FirstName,
                request.LastName,
                request.Email
                );

            UserRelational userRelational = users.Item1;
            UserNoSql userNoSql = users.Item2;

            try
            {
                _authentication.GeneratePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                User.SetLoginForUsers([userRelational, userNoSql], passwordHash, passwordSalt);

                await _userRepository.Add(userRelational,userNoSql);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success(userRelational.Id);
            }
            catch (Exception ex)
            {
                return Result.Failure<Guid>(new Error(ex.Message));
            }
        }
    }
}
