using Models.SubModels;
using Models.SubModels.Account;

namespace Models.Models
{
    public class Account
    {
        public Guid? Id { get; set; }
        public string? Tag { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? IsVerified { get; set; } = false;

        public ICollection<AccountDto>? Follwing { get; set; }
        public ICollection<AccountDto>? Followers { get; set; }

        public ICollection<Post>? Posts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Post>? SavedPosts { get; set; }
        public ICollection<ReactedPost>? ReactedPosts { get; set; }

        public ICollection<Photo>? Photos { get; set; }
    }
}
