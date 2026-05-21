using Microsoft.EntityFrameworkCore;
using SM.Application.Database;
using SM.Domain.Messages;
using SM.Domain.Photos;
using SM.Domain.Users;

namespace SM.Application.IntegrationTests.Users;

public sealed class UserTestData(IDbContextFactory<ApplicationDbContext> contextFactory)
{
    public async Task AddAsync(params User[] users)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        context.Users.AddRange(users);
        await context.SaveChangesAsync();
    }

    public async Task<Guid> CreateProfileUserAsync(string aboutMe, bool withBackgroundPhoto)
    {
        var user = User.Create(new UserDto(
            $"user_{Guid.NewGuid():N}",
            "Test",
            "User",
            $"{Guid.NewGuid():N}@example.com"));

        user.AuthoredPosts.Add(Post.Create(aboutMe, user.Id));

        if (withBackgroundPhoto)
        {
            user.Photos.Add(new Photo(Guid.CreateVersion7())
            {
                UserId = user.Id,
                Type = PhotoType.Background,
                CreatedAt = DateTime.UtcNow
            });
        }

        await AddAsync(user);

        return user.Id;
    }
}
