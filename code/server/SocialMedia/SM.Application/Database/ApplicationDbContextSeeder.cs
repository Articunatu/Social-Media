using Bogus;
using Microsoft.EntityFrameworkCore;
using SM.Domain.Authentication;
using SM.Domain.Messages;
using SM.Domain.Photos;
using SM.Domain.Reactions;
using SM.Domain.Users;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

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

        // Create and persist users first so that relationships can reference existing rows
        var users = CreateUsers(faker);
        context.Users.AddRange(users);
        context.SaveChanges();

        // Create follow relationships after users are persisted
        AddFollowRelationships(faker, users);
        context.SaveChanges();

        // Create and persist posts
        var posts = CreatePosts(faker, users);
        context.Posts.AddRange(posts);
        context.SaveChanges();

        // Create and persist comments
        var comments = CreateComments(faker, posts, users);
        context.Comments.AddRange(comments);
        context.SaveChanges();

        // Create reactions, photos and tokens and persist
        var reactions = CreateReactions(faker, posts, comments, users);
        var photos = CreatePhotos(faker, users).ToList();
        var tokens = CreateTokens(faker, users).ToList();

        context.Reactions.AddRange(reactions);
        context.Photos.AddRange(photos);
        context.Tokens.AddRange(tokens);
        context.SaveChanges();

        // Persist mapping of user tags -> plaintext passwords for developer convenience
        WritePasswordsToFile(users);
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

    private static string Truncate(string value, int max)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= max ? value : value.Substring(0, max);
    }

    private static readonly string[] PredefinedPasswords =
    [
        "Password!1",
    ];

    private static List<User> CreateUsers(Faker faker)
    {
        var users = Enumerable.Range(1, 12)
            .Select(index =>
            {
                var firstName = Truncate(faker.Name.FirstName(), 25);
                var lastName = Truncate(faker.Name.LastName(), 40);

                // Tag column has a max length of 20
                var baseTag = $"user{index}_{firstName}{lastName}".ToLowerInvariant();
                var tag = Truncate(baseTag, 20);

                var email = Truncate($"user{index}@socialmedia.local", 100);

                var user = User.Create(tag, firstName, lastName, email);

                var password = PredefinedPasswords.First();
                var (hash, salt) = HashPassword(password);
                user.SetLogin(hash, salt);

                return user;
            })
            .ToList();

        return users;
    }

    private static (byte[] Hash, byte[] Salt) HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);
        return (hash, salt);
    }

    private static void WritePasswordsToFile(IReadOnlyList<User> users)
    {
        try
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "seed-passwords.json");
            var mappings = users.Select((u, i) => new { Tag = u.Tag, Password = PredefinedPasswords[i % PredefinedPasswords.Length] }).ToList();
            var json = JsonSerializer.Serialize(mappings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json, Encoding.UTF8);
        }
        catch
        {
            // Swallow any errors writing the developer file - seeding itself should not fail because of this
        }
    }

    private static List<Post> CreatePosts(Faker faker, IReadOnlyList<User> users)
    {
        return users
            .SelectMany(user => Enumerable.Range(0, faker.Random.Int(1, 3))
                .Select(_ =>
                {
                    var content = faker.Lorem.Paragraph();
                    content = Truncate(content, 280);

                    var post = Post.Create(content, user.Id);
                    post.TimeStamp = faker.Date.RecentOffset(20);
                    return post;
                }))
            .ToList();
    }

    private static List<Comment> CreateComments(Faker faker, IReadOnlyList<Post> posts, IReadOnlyList<User> users)
    {
        var postList = posts.ToList();
        var userList = users.ToList();

        var comments = Enumerable.Range(0, 24)
            .Select(_ =>
            {
                var post = faker.Random.ListItem(postList);
                var author = faker.Random.ListItem(userList);
                var content = faker.Lorem.Sentence();
                content = Truncate(content, 280);

                var comment = Comment.Create(post.Id, content, author.Id);
                comment.TimeStamp = faker.Date.RecentOffset(10);
                return comment;
            })
            .ToList();

        var replies = comments
            .Take(8)
            .Select(parent =>
            {
                var author = faker.Random.ListItem(userList);
                var content = faker.Lorem.Sentence();
                content = Truncate(content, 280);

                var reply = Comment.Create(parent.ParentPostId, content, author.Id, parent.Id);
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

        var postList = posts.ToList();
        var commentList = comments.ToList();
        var userList = users.ToList();

        // Defensive: Only proceed if posts and users are not empty
        if (postList.Count > 0 && userList.Count > 0)
        {
            int maxPostReactions = Math.Min(20, postList.Count * userList.Count);
            while (reactedPosts.Count < maxPostReactions)
            {
                var post = faker.Random.ListItem(postList);
                var user = faker.Random.ListItem(userList);

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
        }

        // Defensive: Only proceed if comments and users are not empty
        if (commentList.Count > 0 && userList.Count > 0)
        {
            int maxCommentReactions = Math.Min(20, commentList.Count * userList.Count);
            while (reactedComments.Count < maxCommentReactions)
            {
                var comment = faker.Random.ListItem(commentList);
                var user = faker.Random.ListItem(userList);

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
        }

        return reactions;
    }

    private static IEnumerable<Photo> CreatePhotos(Faker faker, IReadOnlyList<User> users)
    {
        return users
            .SelectMany(user => new[]
            {
                new Photo(Guid.CreateVersion7())
                {
                    UserId = user.Id,
                    Type = PhotoType.Profile,
                    FileName = Truncate($"{user.Tag}-profile.jpg", 200), // keep reasonable length
                    ContentType = "image/jpeg",
                    Data = Encoding.UTF8.GetBytes(faker.System.FileName("jpg")),
                    CreatedAt = faker.Date.Recent(30)
                },
                new Photo(Guid.CreateVersion7())
                {
                    UserId = user.Id,
                    Type = PhotoType.Background,
                    FileName = Truncate($"{user.Tag}-background.jpg", 200),
                    ContentType = "image/jpeg",
                    Data = Encoding.UTF8.GetBytes(faker.System.FileName("jpg")),
                    CreatedAt = faker.Date.Recent(30)
                }
            });
    }

    private static IEnumerable<Token> CreateTokens(Faker faker, IReadOnlyList<User> users)
    {
        return users
            .Take(6)
            .Select(user => new Token(Guid.CreateVersion7())
            {
                UserId = user.Id,
                Text = faker.Random.Guid().ToString("N"),
                Created = faker.Date.RecentOffset(2),
                Expires = faker.Date.SoonOffset(14)
            });
    }

    private static void AddFollowRelationships(Faker faker, IReadOnlyList<User> users)
    {
        var userList = users.ToList();

        foreach (var user in userList)
        {
            var targets = userList
                .Where(candidate => candidate.Id != user.Id)
                .OrderBy(_ => faker.Random.Int())
                .Take(3)
                .ToList();

            foreach (var target in targets)
            {
                // Ensure we don't add duplicates
                if (!user.Following.Any(f => f.Id == target.Id))
                {
                    user.Following.Add(target);
                }
            }
        }
    }
}
