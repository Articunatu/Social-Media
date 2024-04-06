
namespace SocialMedia.Domain.Users.ValueObjects
{
    public record Name(string Value)
    {
        public const int MinLength = 1;
        public const int MaxLength = 50;
        public const string Pattern = @"^[a-zA-Z]+$";
    }
}
