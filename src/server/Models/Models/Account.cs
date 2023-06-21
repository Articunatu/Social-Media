using Models.DataTransferObjects;
using Models.SubModels;
using Models.SubModels.Account;
using Newtonsoft.Json;

namespace Models.Models
{
    public class Account
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        public string Tag { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsVerified { get; set; } = false;
        public string Mail { get; set; }
        public Login Login { get; set; }
        public RefreshToken Token { get; set; }
        public Photo? ProfilePicture { get; set; }
        public Photo? BackgroundPicture { get; set; }

        public ICollection<AccountDto>? Follwing { get; set; }
        public ICollection<AccountDto>? Followers { get; set; }

        public ICollection<Post>? Posts { get; set; }
        public ICollection<A_Comment>? Comments { get; set; }
        public ICollection<Post>? SavedPosts { get; set; }
        public ICollection<ReactedPost>? ReactedPosts { get; set; }

        public ICollection<Photo>? Photos { get; set; }
    }
}