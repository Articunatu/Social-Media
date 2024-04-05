using SocialMedia.Application.Abstractions;
using SocialMedia.Application.Authentication;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;
using SocialMedia.Domain.Users.ValueObjects;


namespace SocialMedia.Application.Users.Commands.AddUser
{
    public sealed class AddUserCommandHandler : ICommandHandler<AddUserCommand>
    {
        readonly IUserWriteRepository _userRepository;
        //readonly IAuthenticationService _authenticationService;
        readonly IUnitOfWork _unitOfWork;

        public AddUserCommandHandler(IUserWriteRepository userRepository, IUnitOfWork unitOfWork 
          )
        {
            _userRepository = userRepository;
            //_authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.Create(
                new Tag(request.Tag),
                new Name(request.FirstName),
                new Name(request.LastName),
                new Email(request.Email));
            try
            {
                await _userRepository.Add(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(new Error(e.Message));
            }
        }
    }
}
