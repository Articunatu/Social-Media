using SM.Domain.Abstractions;

namespace SM.Domain
{
    public class User : Entity<Guid>, ISoftDeletable
    {
        public User(Guid id) : base(id) { }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? TimeOfDelete { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }


    }
}
