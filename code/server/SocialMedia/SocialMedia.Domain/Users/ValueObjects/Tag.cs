
namespace SocialMedia.Domain.Users.ValueObjects
{
    public record Tag(string Value)
    {
        public const int MinLength = 1;
        public const int MaxLength = 29;
        public const string Pattern = @"^(?!.*\.\.)(?!.*\.$)[^\W][\w.]$";
    }
}
