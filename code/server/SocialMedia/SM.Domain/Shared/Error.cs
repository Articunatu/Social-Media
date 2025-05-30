namespace SM.Domain.Shared;

public class Error(string header, string message) : IEquatable<Error>
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    public string Header { get; } = header ?? throw new ArgumentNullException(nameof(header));
    public string Message { get; } = message ?? string.Empty;

    public Error(string header)
        : this(header, string.Empty) { }

    public static implicit operator string(Error error) => error.Header;

    public override bool Equals(object? obj)
    {
        return Equals(obj as Error);
    }

    public bool Equals(Error? other)
    {
        if (other is null)
            return false;

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