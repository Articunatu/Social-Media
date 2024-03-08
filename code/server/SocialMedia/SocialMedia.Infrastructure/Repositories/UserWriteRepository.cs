using Microsoft.Azure.Cosmos;
using SocialMedia.Domain.Users;
using User = SocialMedia.Domain.Users.User;

namespace SocialMedia.Infrastructure.Repositories
{
    internal sealed class UserWriteRepository(ApplicationDbContext context, Container container) : 
        WriteRepository<User, Guid>(context, container), IUserWriteRepository { }
}
