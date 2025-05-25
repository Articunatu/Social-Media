using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using User = SocialMedia.Domain.Users.User;
using SocialMedia.Domain.Photos;
using SocialMedia.Application.Abstractions;
using SocialMedia.Application.Profiles.GetProfilePosts;
using SocialMedia.Application.Shared.Models;

namespace SocialMedia.Infrastructure.Profiles;

internal sealed class ProfileRepository(Container container) : IProfileRepository
{
    public async Task<ProfileFeedResponse?> GetPagedProfilePosts(Guid userId, PageFilter filter)
    {
        var query = container.GetItemLinqQueryable<User>(allowSynchronousQueryExecution: false)
            .Where(u => u.Id == userId)
            .Select(u => new
            {
                u.Id,
                u.Tag,
                u.FullName,
                ProfilePhoto = u.Photos.Where(p => p.Type == PhotoType.Profile).Select(p => p),
                Posts = u.Posts
                    .Select(p => new
                    {
                        p.Content,
                        p.TimeStamp,
                        Reactions = p.Reactions
                            .GroupBy(r => r.Type)
                            .Select(g => new { Type = g.Key, Count = g.Count() }),
                        RepliesCount = p.Replys.Count
                    })
            })
            .ToFeedIterator();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            var user = response.Resource.FirstOrDefault();

            if (user is not null)
            {
                var profileInfo = new ProfileInfo(user.Id, user.Tag, user.FullName, user.ProfilePhoto.ToArray()[0]);

                var pagedPosts = user.Posts
                    .Select(p => new ProfilePostDto
                    {
                        Content = p.Content,
                        TimeStamp = p.TimeStamp,
                        ReactionCounts = p.Reactions.ToDictionary(rc => rc.Type, rc => rc.Count),
                        RepliesCount = p.RepliesCount
                    })
                    .ToPagedFeed(filter);

                return new ProfileFeedResponse(profileInfo, pagedPosts);
            }
        }

        return null;
    }
}