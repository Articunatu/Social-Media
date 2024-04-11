using MediatR;
using SocialMedia.Application.Abstractions;
using SocialMedia.Application.Abstractions.Authentication;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;
using System.Security.Cryptography;
using System.Text;


namespace SocialMedia.Application.Users.Commands.AddUserCommand
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
                //var loginId = await _authentication.RegisterAsync(
                //user,
                //request.Password,
                //cancellationToken);

                //user.SetLogin(loginId);

                GeneratePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var authentication = new LoginInformation
                {
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                user.SetLogin(authentication);

                await _userRepository.Add(user);

                await _unitOfWork.SaveChangesAsync();

                return user.Id;
            }
            catch (Exception ex)
            {
                throw;
                //return Result.Failure(new Error(ex.Message));
            }
        }

        private static void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var secutiry = new HMACSHA512();
            passwordSalt = secutiry.Key;
            passwordHash = secutiry.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
