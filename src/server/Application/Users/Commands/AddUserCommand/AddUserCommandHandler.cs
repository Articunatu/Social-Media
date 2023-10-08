using Application.Abstraction;
using Domain.Entities;
using Domain.Shared;
using Infrastructure.Repositories;

namespace Application.Users.Commands.AddUserCommand
{
    public sealed class AddUserCommandHandler : ICommandHandler<AddUserCommand>
    {
        readonly UserRepository _userRepository;

        public AddUserCommandHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {

            var user = new User(Guid.NewGuid())
            {
                Tag = request.Tag,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Mail = request.Email
            };

            await _userRepository.Add(user);
            
            return Result.Success();
        }
    }
}
