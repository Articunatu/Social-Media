using System.Net;

namespace SM.Domain.Shared;

public class Result
{
    protected internal Result(bool isSuccess, Error error, HttpStatusCode status)
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
    public HttpStatusCode Status { get; }

    public static Result Success() => new(true, Error.None, HttpStatusCode.OK);

    public static Result Failure(Error error, HttpStatusCode status) => new(false, error, status);

    public static Result<T> Success<T>(T value) => new(value, true, Error.None, HttpStatusCode.OK);

    public static Result<T> Failure<T>(Error error, HttpStatusCode status) => new(default!, false, error, status);

    public static Result<T> Create<T>(T? value) =>
        value is not null
            ? Success(value)
            : Failure<T>(Error.NullValue, HttpStatusCode.BadRequest);
}
