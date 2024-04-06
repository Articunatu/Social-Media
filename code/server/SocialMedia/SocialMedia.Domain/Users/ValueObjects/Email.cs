
namespace SocialMedia.Domain.Users.ValueObjects
{
    public record Email(string Value)
    {
        public const string Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    }
}