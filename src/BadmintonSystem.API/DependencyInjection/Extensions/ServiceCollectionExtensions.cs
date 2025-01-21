using BadmintonSystem.API.Hubs;
using BadmintonSystem.API.Middleware;
using BadmintonSystem.Application.Abstractions;

namespace BadmintonSystem.API.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiddlewareConfigurationAPI(this IServiceCollection services)
    {
        return services.AddTransient<ExceptionHandlingMiddleware>()
            .AddScoped<IBookingHub, BookingHub>()
            .AddScoped<IChatHub, ChatHub>();
    }
}
