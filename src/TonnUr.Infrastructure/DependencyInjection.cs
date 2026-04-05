using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TonnUr.Application.Abstractions;
using TonnUr.Domain.Communities;
using TonnUr.Infrastructure.Persistance;
using TonnUr.Infrastructure.Services;

namespace TonnUr.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ISlugUniquenessChecker, SlugUniquenessChecker>();
        services.AddSingleton<ISlugGenerator, SlugGenerator>();

        return services;
    }
}