using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;

namespace SocialMedia.Application.Users.AddUser
{
    internal sealed class AddUserCommandHandler : ICommandHandler<AddUserCommand, Guid>
    {
        readonly IUserWriteRepository _userRepository;
        readonly IUnitOfWork _unitOfWork;
        readonly IAuthenticationService _authentication;

        public AddUserCommandHandler(IUserWriteRepository userRepository, IUnitOfWork unitOfWork, IAuthenticationService authentication)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _authentication = authentication;
        }

        public async Task<Result<Guid>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.Create(
                request.Tag,
                request.FirstName,
                request.LastName,
                request.Email
                );

            try
            {
                _authentication.GeneratePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                user.SetLogin(passwordHash, passwordSalt);

                await _userRepository.Add(user);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success(user.Id);
            }
            catch (Exception ex)
            {
                throw;
                //return Result.Failure(new Error(ex.Message));
            }
        }
    }
}
