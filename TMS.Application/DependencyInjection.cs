using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TMS.Application.Common.Behaviors;

namespace TMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        return services;
    }
}