using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TonnUr.Application.Common.Behaviours;

namespace TonnUr.Application;

public static class ApplicationDependencyInjection
{
    private static Assembly ApplicationAssembly => typeof(ApplicationDependencyInjection).Assembly;
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(ApplicationAssembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
        });
        return services;
    }
}