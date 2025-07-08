namespace SM.Domain.Shared;

public class Result
{
    protected internal Result(bool isSuccess, Error error, StatusCode status)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Successful result must not contain an error.");

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Failed result must contain an error.");

        IsSuccess = isSuccess;
        Error = error;
        Status = status;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error { get; }
    public StatusCode Status { get; }

    public static Result Success() => new(true, Error.None, StatusCode.Ok);

    public static Result Failure(Error error, StatusCode status) => new(false, error, status);

    public static Result<T> Success<T>(T value) => new(value, true, Error.None, StatusCode.Ok);

    public static Result<T> Failure<T>(Error error, StatusCode status) => new(default!, false, error, status);

    public static Result<T> Create<T>(T? value) =>
        value is not null
            ? Success(value)
            : Failure<T>(Error.NullValue, StatusCode.Validation);
}
