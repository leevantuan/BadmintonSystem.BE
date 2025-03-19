using BadmintonSystem.API.Hubs;
using BadmintonSystem.Application.Abstractions;

namespace BadmintonSystem.API.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiddlewareConfigurationAPI(this IServiceCollection services)
    {
        return services.AddScoped<IBookingHub, BookingHub>()
            .AddScoped<IRegisterHub, RegisterHub>()
            .AddScoped<IChatHub, ChatHub>();
    }
}
