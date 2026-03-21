using TonnUr.Api.Auth;

namespace TonnUr.Api;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        return services
            .AddHttpContextAccessor()
            .AddScoped<CurrentUser>();
    }
}