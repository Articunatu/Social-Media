using SM.Domain.Shared;

namespace SM.WebApi.Extensions;

public static class ResultExtensions
{
    public static IResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        var status = result.Status;

        return Results.Problem(
            detail: string.Join("; ", result.Error),
            statusCode: (int)status,
            title: status.ToTitle(),
            type: $"https://httpstatuses.com/{status}"
        );
    }
}
