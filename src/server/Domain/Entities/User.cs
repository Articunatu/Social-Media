using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public sealed class User : Entity
    {
        public User(Guid id) : base(id) { }

        [Required]
        [RegularExpression(@"^[a-z0-9_-]{3,15}$")]
        public string Tag { get; set; }

        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        //password regex
        //
    }
}
