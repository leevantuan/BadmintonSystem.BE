using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Infrastructure.Authentication;
using BadmintonSystem.Infrastructure.DependencyInjection.Options;
using BadmintonSystem.Infrastructure.Seed;
using BadmintonSystem.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BadmintonSystem.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServicesInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IJwtTokenService, JwtTokenService>()
            .AddTransient<IGmailService, GmailService>()
            .AddTransient<IRedisService, RedisService>()
            .AddTransient<IDbSeeder, DbSeeder>();
    }

    public static void AddRedisInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Đọc cấu hình Redis từ appsettings.json
        var env = configuration["Environment"] ?? "Development";
        _ = env.Equals("Development", StringComparison.OrdinalIgnoreCase);

        var redisOption = new RedisOption();
        configuration.GetSection("RedisOption").Bind(redisOption);
        services.AddSingleton(redisOption);

        // ✅ Đăng ký StackExchange Redis Cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisOption.ConnectionString;
            options.InstanceName = redisOption.InstanceName;
        });

        // ✅ Đăng ký Redis ConnectionMultiplexer & RedisService
        if (redisOption.Enabled)
        {
            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(redisOption.ConnectionString));

            services.AddTransient<IRedisService, RedisService>();
        }
    }
}
