using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Messages;
using SocialMedia.Domain.Reactions;
using SocialMedia.Domain.Users.Events;
using System.Text.RegularExpressions;

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

        private string _firstName;
        private string _lastName;
        private string _email;

        public string Tag { get; set; }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (Regex.IsMatch(value, @"^[a-zA-Z]+$")) // Allows only letters
                    _firstName = value;
                else
                    throw new ArgumentException("First name must contain only letters.");
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (Regex.IsMatch(value, @"^[a-zA-Z]+$")) // Allows only letters
                    _lastName = value;
                else
                    throw new ArgumentException("Last name must contain only letters.");
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if(value is not null)
                {
                    if (Regex.IsMatch(value, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")) // Validates email format
                        _email = value;
                }
                else
                    _email = "felformat@hotmail.com";
            }
        }

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
    }
}
