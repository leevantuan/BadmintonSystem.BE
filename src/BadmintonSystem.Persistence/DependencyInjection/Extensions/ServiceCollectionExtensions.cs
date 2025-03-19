using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Persistence.DependencyInjection.Options;
using BadmintonSystem.Persistence.Interceptors;
using BadmintonSystem.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace BadmintonSystem.Persistence.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    private const string DefaultConnection = "PostgresConnectionStrings";

    public static void AddPostgresConfigurationPersistence(this IServiceCollection services)
    {
        // Enable legacy timestamp behavior for Npgsql
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        RegisterDbContextWithPool<ApplicationDbContext>(services, nameof(DefaultConnection));
        RegisterDbContextWithPool<TenantDbContext>(services, nameof(DefaultConnection));

        // Configure Identity
        services.AddIdentityCore<AppUser>()
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        // Configure Identity options
        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.AllowedForNewUsers = true; // Default true
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Default 5
            options.Lockout.MaxFailedAccessAttempts = 3; // Default 5

            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });
    }

    private static void RegisterDbContextWithPool<TContext>(IServiceCollection services, string connectionName)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>((provider, builder) =>
        {
            UpdateAuditableEntitiesInterceptor auditableInterceptor =
                provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsMonitor<PostgresServerRetryOptions>>();

            var env = configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development";
            _ = env.Equals("Development", StringComparison.OrdinalIgnoreCase);

            builder.EnableDetailedErrors()
                   .EnableSensitiveDataLogging()
                   .UseLazyLoadingProxies()
                   .UseNpgsql(
                       configuration.GetConnectionString(connectionName),
                       optionsBuilder => optionsBuilder.ExecutionStrategy(
                           dependencies => new NpgsqlRetryingExecutionStrategy(
                               dependencies,
                               options.CurrentValue.MaxRetryCount,
                               options.CurrentValue.MaxRetryDelay,
                               options.CurrentValue.ErrorNumbersToAdd
                           ))
                       .MigrationsAssembly(typeof(TContext).Assembly.GetName().Name))
                   .AddInterceptors(auditableInterceptor);
        });
    }

    public static OptionsBuilder<PostgresServerRetryOptions> AddPostgresServerRetryOptionsConfigurationPersistence
        (this IServiceCollection services, IConfigurationSection section)
    {
        return services
            .AddOptions<PostgresServerRetryOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    public static void AddRepositoryBaseConfigurationPersistence(this IServiceCollection services)
    {
        services
            //.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork))
            .AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
    }

    public static void AddInterceptorConfigurationPersistence(this IServiceCollection services)
    {
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
    }

    public static IServiceCollection AddMultipleDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        // Apply migration on tenant database ( central database )
        using IServiceScope serviceScope = services.BuildServiceProvider().CreateScope();
        TenantDbContext tenantDbContext = serviceScope.ServiceProvider.GetRequiredService<TenantDbContext>();

        if (tenantDbContext.Database.GetPendingMigrations().Any())
        {
            tenantDbContext.Database.Migrate(); // Apply migration on tenant Basse Database
        }

        // Get of list tenants
        List<Tenant> tenants = tenantDbContext.Tenants.ToList();
        // Read default connection string
        string defaultConnection = configuration.GetConnectionString("DefaultConnection");

        // Loop through tenants, apply migration on applicationContext
        foreach (Tenant tenant in tenants)
        {
            string connectionString = string.IsNullOrEmpty(tenant.ConnectionString) ? defaultConnection : tenant.ConnectionString;

            // Aplication db context ( app per tenant )
            using IServiceScope scopeApplication = services.BuildServiceProvider().CreateScope();
            ApplicationDbContext applicationContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            applicationContext.Database.SetConnectionString(connectionString);
            if (applicationContext.Database.GetPendingMigrations().Any())
            {
                applicationContext.Database.Migrate();
            }
        }

        return services;
    }
}
