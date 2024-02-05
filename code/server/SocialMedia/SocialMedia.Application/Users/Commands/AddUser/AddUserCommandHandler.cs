using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;


namespace SocialMedia.Application.Users.Commands.AddUser
{
    public sealed class AddUserCommandHandler : ICommandHandler<AddUserCommand>
    {
        readonly IUserWriteRepository _userRepository;
        readonly IUnitOfWork _unitOfWork;

        public AddUserCommandHandler(IUserWriteRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {

            var user = new User(
                Guid.NewGuid(),
                request.Tag,
                request.FirstName,
                request.LastName,
                request.Email
                );

            await _userRepository.Add(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
