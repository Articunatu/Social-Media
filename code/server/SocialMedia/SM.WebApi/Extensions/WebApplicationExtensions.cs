using FluentValidation;
using SM.WebApi.Endpoints;
using System.Data;

namespace SM.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static void MapApiEndpoints(this WebApplication app)
    {
        app.MapAuthenticationEndpoints();
        app.MapCommentEndpoints();
        app.MapPostEndpoints();
        app.MapReactionEndpoints();
        app.MapUserEndpoints();
    }

    //public static void SetExceptionHandling(this WebApplication app)
    //{
    //    app.UseExceptionHandler(new ExceptionHandlerOptions
    //    {
    //        StatusCodeSelector = ex => ex switch
    //        {
    //            DuplicateNameException => StatusCodes.Status409Conflict,
    //            ValidationException => StatusCodes.Status400BadRequest,
    //            InvalidCastException => StatusCodes.Status400BadRequest,
    //            ArgumentException => StatusCodes.Status400BadRequest,
    //            _ => StatusCodes.Status500InternalServerError
    //        }
    //    });
    //}
}
