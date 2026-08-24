using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SM.Application.Behaviors;
using SM.Application.Posts.GetProfilePosts;
using SM.Infrastructure.Behaviors;
using SM.Application.Database;
using SM.Application.Authentication;
using SM.Infrastructure.Authentication;
using System.Text;

namespace SM.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment? env = null)
    {
        if (env?.IsEnvironment("Testing") == true)
        {
            // For testing, skip database registration here - it will be configured in the test fixture
            return services.AddDatabaseAgnosticServices(config);
        }

        services.AddDbContextFactory<IdentityDbContext>((serviceProvider, options) =>
            options.UseSqlServer(config.GetConnectionString("EfcoreSocials")));

        services.AddDbContextFactory<ContentDbContext>((serviceProvider, options) =>
            options.UseSqlServer(config.GetConnectionString("EfcoreSocials")));

        services.AddDbContextFactory<SocialGraphDbContext>((serviceProvider, options) =>
            options.UseSqlServer(config.GetConnectionString("EfcoreSocials")));

        services.AddDbContextFactory<FeedDbContext>((serviceProvider, options) =>
            options.UseSqlServer(config.GetConnectionString("EfcoreSocials")));

        services.AddDbContextFactory<MediaDbContext>((serviceProvider, options) =>
            options.UseSqlServer(config.GetConnectionString("EfcoreSocials")));

        return services.AddDatabaseAgnosticServices(config);
    }

    private static IServiceCollection AddDatabaseAgnosticServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly, includeInternalTypes: true);

        services.AddTransient<IJwtService, JwtService>();

        services.AddMemoryCache();
        services.AddTransient(typeof(ICachingBehavior<,>), typeof(CachingBehavior<,>));
        services.AddTransient<ILoggingBehaviour, LoggingBehaviour>();
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(GetProfilePostsQuery).Assembly);
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        return services;
    }
    public static void AddAuthenticationServices(this IServiceCollection services, string jwtSettingsTokenKey)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettingsTokenKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
             });
        });
    }

    public static void AddValidationProblems(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Title = "Validation failed",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://httpstatuses.com/400"
                };

                return new BadRequestObjectResult(problemDetails);
            };
        });
    }
}
