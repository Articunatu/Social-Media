using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects.User
{
    public sealed class FirstName : ValueObject
    {
        public string Value { get; }

        public const int MaxLength = 20;

        public FirstName(string value)
        {
            Value = value;
        }

        public static Result<FirstName> Create(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return Result.Failure<FirstName>(new Error(
                    "FirstName.Empty",
                    "First name is empty"));
            }

            if (firstName.Length > MaxLength)
            {
                return Result.Failure<FirstName>(new Error(
                    "FirstName.TooLong",
                    "First name is too long"));
            }

            return new FirstName(firstName);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}