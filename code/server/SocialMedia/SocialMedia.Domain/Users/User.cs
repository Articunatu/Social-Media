using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Messages;
using SocialMedia.Domain.Messages.DirectMessages;
using SocialMedia.Domain.Photos;
using SocialMedia.Domain.Reactions;
using SocialMedia.Domain.Users.Events;

namespace SocialMedia.Domain.Users
{
    public class User: Entity<Guid>, ISoftDeletable
    {
        public User() { }

        public User(Guid id, string tag, string firstName, string lastName, string email) : base(id)
        {
            Tag = tag;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public string Tag { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? TimeOfDelete { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Token Token { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<DirectMessage>? DirectMessages { get; set; }
        public ICollection<Photo>? Photos { get; set; }

        public static (UserRelational, UserNoSql) Create(string tag, string firstName, string lastName, string email)
        {
            var userId = Guid.NewGuid();

            var userRelational = new UserRelational(userId, tag, firstName, lastName, email);
            userRelational.RaiseDomainEvent(new UserCreatedDomainEvent(userRelational.Id));

            var userNoSql = new UserNoSql(userId, tag, firstName, lastName, email);

            return (userRelational, userNoSql);
        }


        public static FollowUser Follow(Guid followerId, Guid followingId, UserRelational follower, UserRelational following)
        {
            var followRef = new FollowUser(Guid.NewGuid(), followerId, followingId, follower, follower);
            follower.RaiseDomainEvent(new UserFollowedDomainEvent(follower.Id));
            following.RaiseDomainEvent(new UserFollowedDomainEvent(following.Id));
            return followRef;
        }

        public void SetLogin(byte[] passwordHash, byte[] passwordSalt)
        {
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public static void SetLoginForUsers(IEnumerable<User> users, byte[] passwordHash, byte[] passwordSalt)
        {
            foreach (var user in users)
                user.SetLogin(passwordHash, passwordSalt);
        }

    }

    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Tag { get; set; }
        public string Fullname { get; set; }
        public Photo ProfilePhoto { get; set; }
        //public string Description { get; set; }
    }

    public sealed class UserRelational : User
    {
        public UserRelational(Guid id, string tag, string firstName, string lastName, string email)
        : base(id, tag, firstName, lastName, email) { }
        public ICollection<Reply>? Replies { get; set; }
        public ICollection<ReactionRelational>? Reactions { get; set; }
        public ICollection<UserRelational> Followers { get; set; }
        public ICollection<UserRelational> Following { get; set; }
    }

    public sealed class UserNoSql : User
    {
        public UserNoSql(Guid id, string tag, string firstName, string lastName, string email)
        : base(id, tag, firstName, lastName, email) { }
        public ICollection<Guid>? Replies { get; set; }
        public ICollection<UserDTO>? Followers { get; set; }
        public ICollection<UserDTO>? Following { get; set; }
        public ICollection<ReactionNoSQL> Reactions { get; set; }

        public UserDTO Follow(User follower, User following, Guid followerId, Guid followingId)
        {
            return UserDTO(follower, following, followerId, followingId);
        }
    }
}