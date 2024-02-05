﻿using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using SocialMedia.Application.Behaviors;

namespace SocialMedia.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(
                    typeof(DependencyInjection).Assembly);

                configuration.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            });

            services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

            return services;
        }
    }
}