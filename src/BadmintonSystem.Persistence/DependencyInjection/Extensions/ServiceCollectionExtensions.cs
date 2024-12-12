using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Persistence.DependencyInjection.Options;
using BadmintonSystem.Persistence.Interceptors;
using BadmintonSystem.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
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
    private const string SqlConnectionStrings = "SqlConnectionStrings";
    private const string PostgresConnectionStrings = "PostgresConnectionStrings";

    public static void AddSqlConfigurationPersistence(this IServiceCollection services)
    {
        // use db context pool => not use normal db context
        services.AddDbContextPool<DbContext, ApplicationDbContext>((provider, builder) =>
        {
            var auditableInterceptor = provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>();

            builder
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging(true)
                .UseLazyLoadingProxies(true) // if you use Lazy Loading, all of the navigation fields should be VIRTUAL
                .UseSqlServer(
                    connectionString: configuration.GetConnectionString(nameof(SqlConnectionStrings)),
                    sqlServerOptionsAction: optionsBuider
                        => optionsBuider.ExecutionStrategy(
                                            dependencies => new SqlServerRetryingExecutionStrategy(
                                                    dependencies: dependencies,
                                                    maxRetryCount: options.CurrentValue.MaxRetryCount,
                                                    maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                                    errorNumbersToAdd: options.CurrentValue.ErrorNumbersToAdd
                                            ))
                                        .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
                .AddInterceptors(auditableInterceptor);
        });

        services.AddIdentityCore<AppUser>()
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

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

    public static OptionsBuilder<SqlServerRetryOptions> AddSqlServerRetryOptionsConfigurationPersistence(this IServiceCollection services, IConfigurationSection section)
        => services
                .AddOptions<SqlServerRetryOptions>()
                .Bind(section)
                .ValidateDataAnnotations()
                .ValidateOnStart();

    public static void AddPostgresConfigurationPersistence(this IServiceCollection services)
    {
        // Enable legacy timestamp behavior for Npgsql
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // use db context pool => not use normal db context
        services.AddDbContextPool<DbContext, ApplicationDbContext>((provider, builder) =>
        {
            var auditableInterceptor = provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsMonitor<PostgresServerRetryOptions>>();

            builder
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging(true)
                .UseLazyLoadingProxies(true) // if you use Lazy Loading, all of the navigation fields should be VIRTUAL
                .UseNpgsql(
                    connectionString: configuration.GetConnectionString(nameof(PostgresConnectionStrings)),
                    npgsqlOptionsAction: optionsBuider
                        => optionsBuider.ExecutionStrategy(
                                            dependencies => new NpgsqlRetryingExecutionStrategy(
                                                    dependencies: dependencies,
                                                    maxRetryCount: options.CurrentValue.MaxRetryCount,
                                                    maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                                    errorCodesToAdd: options.CurrentValue.ErrorNumbersToAdd
                                            ))
                                        .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
                .AddInterceptors(auditableInterceptor);
        });

        services.AddIdentityCore<AppUser>()
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

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

    public static OptionsBuilder<PostgresServerRetryOptions> AddPostgresServerRetryOptionsConfigurationPersistence(this IServiceCollection services, IConfigurationSection section)
        => services
                .AddOptions<PostgresServerRetryOptions>()
                .Bind(section)
                .ValidateDataAnnotations()
                .ValidateOnStart();

    public static void AddRepositoryBaseConfigurationPersistence(this IServiceCollection services)
        => services
                //.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork))
                .AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));

    public static void AddInterceptorConfigurationPersistence(this IServiceCollection services)
    {
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddScoped<ApplicationDbContextInitialiser>();
    }

    public static async Task<IApplicationBuilder> AddInitialiserConfigurationPersistence(
            this IApplicationBuilder app)
    {
        // Add scoped service in Program.cs instead
        using var scope = app.ApplicationServices.CreateScope();
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();

        return app;
    }
}
