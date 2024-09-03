using BadmintonSystem.API.Middleware;

namespace BadmintonSystem.API.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigureMiddleware(this IServiceCollection services)
        => services.AddTransient<ExceptionHandlingMiddleware>();
}
