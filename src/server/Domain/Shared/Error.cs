namespace Domain.Shared
{
    public class Error : IEquatable<Error>
    {
        public static readonly Error None = new Error(string.Empty, string.Empty);
        public static readonly Error NullValue = new Error("Error.NullValue", "The specified result value is null.");

        public string Header { get; }
        public string Message { get; }

        public Error(string header, string message)
        {
            Header = header;
            Message = message;
        }

        public Error(string header)
        {
            Header = header;
        }

        public static implicit operator string(Error error) => error.Header;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals((Error)obj);
        }

        public bool Equals(Error other)
        {
            if (ReferenceEquals(this, other))
                return true;

            return Header == other.Header && Message == other.Message;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Message);
        }

        public static bool operator ==(Error? a, Error? b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Error? a, Error? b)
        {
            return !(a == b);
        }
    }
}