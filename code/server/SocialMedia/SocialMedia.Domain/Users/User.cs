using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Messages;
using SocialMedia.Domain.Reactions;
using SocialMedia.Domain.Users.Events;
using SocialMedia.Domain.Users.ValueObjects;

namespace SocialMedia.Domain.Users
{
    public sealed class User : Entity<Guid>, ISoftDeletable
    {
        private User() { }

        private User(Guid id, Tag tag, Name firstName, Name lastName, Email email) : base(id)
        {
            Tag = tag;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public Tag Tag { get; private set; }
        public Name FirstName { get; private set; }
        public Name LastName { get; private set; }
        public Email Email { get; private set; }

        public ICollection<FollowUser>? Followers { get; set; }
        public ICollection<FollowUser>? Following { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Reply>? Replies { get; set; }
        public ICollection<Reaction>? Reactions { get; set; }
        public Login Login { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? TimeOfDelete { get; set; }

        public static User Create(Tag tag, Name firstname, Name lastName, Email email)
        {
            var user = new User(Guid.NewGuid(), tag, firstname, lastName, email);
            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
            return user;
        }

        public void SetLogin(Login login)
        {
            Login = login;
        }
    }
}
