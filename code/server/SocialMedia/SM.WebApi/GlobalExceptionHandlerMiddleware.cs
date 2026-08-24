using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SM.WebApi;

internal sealed class GlobalExceptionHandler(IProblemDetailsService service, ILogger<GlobalExceptionHandler> logger) 
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception ex, CancellationToken ct)
    {
        logger.LogError(ex, "Unhandled exception occurred {ErrMsg}", ex.Message);

        httpContext.Response.StatusCode = ex switch
        {
            ApplicationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return await service.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = ex,
            ProblemDetails = new ProblemDetails
            {
                Type = ex.GetType().Name,
                Title = "An error occured",
                Detail = ex.Message,
                Status = httpContext.Response.StatusCode,
                Instance = httpContext.Request.Path
            }
        });
    }
}