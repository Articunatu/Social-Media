using SM.Application;
using SM.Application.Shared.Errors;
using SM.WebApi.Extensions;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapApiEndpoints();

app.UseStatusCodePages();

app.UseExceptionHandler(new ExceptionHandlerOptions
{
    StatusCodeSelector = ex => ex switch
    {
        InvalidCastException => StatusCodes.Status400BadRequest,
        ArgumentException => StatusCodes.Status400BadRequest,
        NotFoundException => StatusCodes.Status404NotFound,
        DuplicateNameException => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    }
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.Run();
