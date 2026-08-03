using Microsoft.AspNetCore.Diagnostics;
using SM.Application;
using SM.WebApi;
using SM.WebApi.Extensions;
using SM.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration, builder.Environment);
builder.Services.AddProblemDetails();
builder.Services.AddValidationProblems();
builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthenticationServices(builder.Configuration["JwtSettings:TokenKey"]!);
builder.Services.AddAuthorization();

string CorsPolicy = nameof(CorsPolicy);

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    app.Services.SeedDatabase();
}

app.MapApiEndpoints();
app.UseExceptionHandler();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseStatusCodePages();

app.UseCors(CorsPolicy);
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

public partial class Program;
