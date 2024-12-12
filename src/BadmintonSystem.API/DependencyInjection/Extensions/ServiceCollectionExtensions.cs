using BadmintonSystem.API.Middleware;

namespace BadmintonSystem.API.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiddlewareConfigurationAPI(this IServiceCollection services)
        => services.AddTransient<ExceptionHandlingMiddleware>();
}
