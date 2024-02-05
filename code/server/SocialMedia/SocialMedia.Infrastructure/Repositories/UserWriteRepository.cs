using Microsoft.Azure.Cosmos;
using SocialMedia.Domain.Users;

namespace SocialMedia.Infrastructure.Repositories
{
    internal sealed class UserWriteRepository(ApplicationDbContext context, Container container) : 
        WriteRepository<User, Guid>(context, container), IUserWriteRepository { }
}
