using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Messages;
using SocialMedia.Domain.Reactions;
using SocialMedia.Domain.Users.Events;
using SocialMedia.Domain.Users.ValueObjects;

namespace SocialMedia.Domain.Users
{
    public sealed class User : Entity<Guid>
    {
        private User() { }

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
        public LoginInformation LoginInformation { get; set; }
        public RefreshToken Token { get; set; }

        public ICollection<FollowUser>? Followers { get; set; }
        public ICollection<FollowUser>? Following { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Reply>? Replies { get; set; }
        public ICollection<Reaction>? Reactions { get; set; }

        public static User Create(string tag, string firstname, string lastName, string email)
        {
            var user = new User(Guid.NewGuid(), tag, firstname, lastName, email);
            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
            return user;
        }

        public void SetLogin(LoginInformation loginInformation)
        {
            LoginInformation = loginInformation;
        }
    }
}
