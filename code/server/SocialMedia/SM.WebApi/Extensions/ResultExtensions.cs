using SM.Domain.Shared;

namespace SM.WebApi.Extensions;

public static class ResultExtensions
{
    public static IResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return TypedResults.Ok(result.Value);

        var status = result.Status;

        return TypedResults.Problem(
            detail: string.Join("; ", result.Error),
            statusCode: (int)status,
            title: status.ToTitle(),
            type: $"https://httpstatuses.com/{status}"
        );
    }

    public static IResult ToActionResult<T>(this Result<T> result, Func<T, IResult> onSuccess)
    {
        if (result.IsSuccess)
            return onSuccess(result.Value);

        var status = result.Status;

        return TypedResults.Problem(
            detail: string.Join("; ", result.Error),
            statusCode: (int)status,
            title: status.ToTitle(),
            type: $"https://httpstatuses.com/{status}"
        );
    }
}
