using Microsoft.AspNetCore.Diagnostics;
using SM.WebApi;
using SM.WebApi.Extensions;
using SM.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddValidationProblems();
builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthenticationServices(builder.Configuration["JwtSettings:TokenKey"]!);
builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.MapApiEndpoints();
app.UseExceptionHandler();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseStatusCodePages();

app.UseCors("MyCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media V1");
        c.RoutePrefix = "swagger";
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.Run();
