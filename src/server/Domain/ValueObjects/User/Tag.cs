using System.Text.RegularExpressions;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects.User
{
    public class Tag : ValueObject
    {
        public string Value { get; }

        public const string RegexPattern = @"^[a-z0-9_-]{3,15}$";
        static readonly Regex Regex = new(RegexPattern);

        public Tag(string value)
        {
            Value = value;
        }

        public static Result<Tag> Create(string tag)
        {
            if (!Regex.IsMatch(tag))
            {
                return Result.Failure<Tag>(new Error(
                    "Tag.Invalid", 
                    "Tag has to consist of 3-15 characters, and the characters can only consist of letters, numbers, hyphen and underscore. " +
                    "Empty space and other characters are unsupported.")); 
            }
            return new Tag(tag);
            
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
