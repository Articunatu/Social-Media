using Microsoft.EntityFrameworkCore;
using SM.Application.Database;
using SM.Domain.Content;
using SM.Domain.Photos;
using SM.Domain.Users;

namespace SM.Application.IntegrationTests.Users;

public sealed class UserTestData(
    IDbContextFactory<IdentityDbContext> identityContextFactory,
    IDbContextFactory<ContentDbContext> contentContextFactory,
    IDbContextFactory<MediaDbContext> mediaContextFactory)
{
    public async Task AddAsync(params User[] users)
    {
        await using var context = await identityContextFactory.CreateDbContextAsync();

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

        await AddAsync(user);

        if (withBackgroundPhoto)
        {
            await using var mediaContext = await mediaContextFactory.CreateDbContextAsync();
            mediaContext.Photos.Add(new Photo(Guid.CreateVersion7())
            {
                UserId = user.Id,
                Type = PhotoType.Background,
                CreatedAt = DateTime.UtcNow
            });
            await mediaContext.SaveChangesAsync();
        }

        await using var contentContext = await contentContextFactory.CreateDbContextAsync();
        contentContext.Posts.Add(Post.Create(aboutMe, user.Id));
        await contentContext.SaveChangesAsync();

        return user.Id;
    }
}
