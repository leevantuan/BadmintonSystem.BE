using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Infrastructure.Authentication;
using BadmintonSystem.Infrastructure.Seed;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonSystem.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServicesInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IJwtTokenService, JwtTokenService>()
            .AddTransient<IDbSeeder, DbSeeder>();
    }
}
