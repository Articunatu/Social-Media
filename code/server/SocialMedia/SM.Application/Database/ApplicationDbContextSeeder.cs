using Bogus;
using Microsoft.EntityFrameworkCore;
using SM.Domain.Authentication;
using SM.Domain.Messages;
using SM.Domain.Photos;
using SM.Domain.Reactions;
using SM.Domain.Users;
using System.Security.Cryptography;
using System.Text;

namespace SM.Application.Database;

public static class ApplicationDbContextSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Users.Any())
        {
            return;
        }

        Randomizer.Seed = new Random(73425);
        var faker = new Faker();

        var users = CreateUsers(faker);
        var posts = CreatePosts(faker, users);
        var comments = CreateComments(faker, posts, users);
        var reactions = CreateReactions(faker, posts, comments, users);
        var photos = CreatePhotos(faker, users);
        var tokens = CreateTokens(faker, users);

        AddFollowRelationships(faker, users);

        context.Users.AddRange(users);
        context.Posts.AddRange(posts);
        context.Comments.AddRange(comments);
        context.Reactions.AddRange(reactions);
        context.Photos.AddRange(photos);
        context.Tokens.AddRange(tokens);
        context.SaveChanges();
    }

    public static async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        if (await context.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        Seed(context);
        await Task.CompletedTask;
    }

    private static List<User> CreateUsers(Faker faker)
    {
        return Enumerable.Range(1, 12)
            .Select(index =>
            {
                var firstName = faker.Name.FirstName();
                var lastName = faker.Name.LastName();
                var user = User.Create(
                    $"user{index}_{firstName}{lastName}".ToLowerInvariant(),
                    firstName,
                    lastName,
                    $"user{index}@socialmedia.local");

                user.SetLogin(
                    SHA256.HashData(Encoding.UTF8.GetBytes($"password-{index}")),
                    SHA256.HashData(Encoding.UTF8.GetBytes($"salt-{index}")));

                return user;
            })
            .ToList();
    }

    private static List<Post> CreatePosts(Faker faker, IReadOnlyList<User> users)
    {
        return users
            .SelectMany(user => Enumerable.Range(0, faker.Random.Int(1, 3))
                .Select(_ =>
                {
                    var post = Post.Create(faker.Lorem.Paragraph(), user.Id);
                    post.TimeStamp = faker.Date.RecentOffset(20);
                    return post;
                }))
            .ToList();
    }

    private static List<Comment> CreateComments(Faker faker, IReadOnlyList<Post> posts, IReadOnlyList<User> users)
    {
        var comments = Enumerable.Range(0, 24)
            .Select(_ =>
            {
                var post = faker.Random.ListItem(posts);
                var author = faker.Random.ListItem(users);
                var comment = Comment.Create(post.Id, faker.Lorem.Sentence(), author.Id);
                comment.TimeStamp = faker.Date.RecentOffset(10);
                return comment;
            })
            .ToList();

        var replies = comments
            .Take(8)
            .Select(parent =>
            {
                var author = faker.Random.ListItem(users);
                var reply = Comment.Create(parent.ParentPostId, faker.Lorem.Sentence(), author.Id, parent.Id);
                reply.TimeStamp = faker.Date.RecentOffset(5);
                return reply;
            });

        comments.AddRange(replies);
        return comments;
    }

    private static List<Reaction> CreateReactions(Faker faker, IReadOnlyList<Post> posts, IReadOnlyList<Comment> comments, IReadOnlyList<User> users)
    {
        var reactions = new List<Reaction>();
        var reactedPosts = new HashSet<(Guid PostId, Guid UserId)>();
        var reactedComments = new HashSet<(Guid CommentId, Guid UserId)>();

        while (reactedPosts.Count < 20)
        {
            var post = faker.Random.ListItem(posts);
            var user = faker.Random.ListItem(users);

            if (!reactedPosts.Add((post.Id, user.Id)))
            {
                continue;
            }

            reactions.Add(new Reaction(Guid.CreateVersion7())
            {
                Type = faker.Random.Enum<ReactionType>(),
                UserId = user.Id,
                PostId = post.Id
            });
        }

        while (reactedComments.Count < 20)
        {
            var comment = faker.Random.ListItem(comments);
            var user = faker.Random.ListItem(users);

            if (!reactedComments.Add((comment.Id, user.Id)))
            {
                continue;
            }

            reactions.Add(new Reaction(Guid.CreateVersion7())
            {
                Type = faker.Random.Enum<ReactionType>(),
                UserId = user.Id,
                CommentId = comment.Id
            });
        }

        return reactions;
    }

    private static List<Photo> CreatePhotos(Faker faker, IReadOnlyList<User> users)
    {
        return users
            .SelectMany(user => new[]
            {
                new Photo(Guid.CreateVersion7())
                {
                    UserId = user.Id,
                    Type = PhotoType.Profile,
                    FileName = $"{user.Tag}-profile.jpg",
                    ContentType = "image/jpeg",
                    Data = Encoding.UTF8.GetBytes(faker.System.FileName("jpg")),
                    CreatedAt = faker.Date.Recent(30)
                },
                new Photo(Guid.CreateVersion7())
                {
                    UserId = user.Id,
                    Type = PhotoType.Background,
                    FileName = $"{user.Tag}-background.jpg",
                    ContentType = "image/jpeg",
                    Data = Encoding.UTF8.GetBytes(faker.System.FileName("jpg")),
                    CreatedAt = faker.Date.Recent(30)
                }
            })
            .ToList();
    }

    private static List<Token> CreateTokens(Faker faker, IReadOnlyList<User> users)
    {
        return users
            .Take(6)
            .Select(user => new Token(Guid.CreateVersion7())
            {
                UserId = user.Id,
                Text = faker.Random.Guid().ToString("N"),
                Created = faker.Date.RecentOffset(2),
                Expires = faker.Date.SoonOffset(14)
            })
            .ToList();
    }

    private static void AddFollowRelationships(Faker faker, IReadOnlyList<User> users)
    {
        foreach (var user in users)
        {
            var targets = users
                .Where(candidate => candidate.Id != user.Id)
                .OrderBy(_ => faker.Random.Int())
                .Take(3)
                .ToList();

            foreach (var target in targets)
            {
                user.Following.Add(target);
            }
        }
    }
}
