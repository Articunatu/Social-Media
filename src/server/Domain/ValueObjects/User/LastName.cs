using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects.User
{
    public class LastName : ValueObject
    {
        public string Value { get; }

        public const int MaxLength = 30;

        public LastName(string value)
        {
            Value = value;
        }

        public static Result<LastName> Create(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return Result.Failure<LastName>(new Error(
                    "LastName.Empty",
                    "First name is empty"));
            }

            if (lastName.Length > MaxLength)
            {
                return Result.Failure<LastName>(new Error(
                    "LastName.TooLong",
                    "First name is too long"));
            }

            return new LastName(lastName);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
