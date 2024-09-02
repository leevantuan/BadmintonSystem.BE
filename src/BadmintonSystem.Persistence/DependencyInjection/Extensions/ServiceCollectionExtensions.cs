using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Persistence.DependencyInjection.Options;
using BadmintonSystem.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BadmintonSystem.Persistence.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSqlConfiguration(this IServiceCollection services)
    {
        // Using DbContextPool
        // Pool là bể chứa các DbContext, If not using nó sẽ giải phóng và return in DbContextPool
        // If need then mình vào lấy use, nó luôn luôn có không cần phải Generate lại
        // Sau 1 time nhất định, If not using then cancel db
        services.AddDbContextPool<DbContext, ApplicationDbContext>((provider, builder) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>(); // Khi khai báo biến Default đặt thành Read-Only

            #region ============== SQL-SERVER-STRATEGY-1 ==============

            builder
          .EnableDetailedErrors(true)
          .EnableSensitiveDataLogging(true)
          .UseLazyLoadingProxies(true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
          .UseSqlServer(
              connectionString: configuration.GetConnectionString("ConnectionStrings"),
              sqlServerOptionsAction: optionsBuilder
                      => optionsBuilder.ExecutionStrategy(
                              dependencies => new SqlServerRetryingExecutionStrategy(
                                  dependencies: dependencies,
                                  maxRetryCount: options.CurrentValue.MaxRetryCount, // It will retry bao nhiêu lần
                                  maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                  errorNumbersToAdd: options.CurrentValue.ErrorNumbersToAdd)) // If it have error then thêm vào
                          .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)); // Generate file Migration ở tại ApplicationDbcontext in Assembly

            #endregion ============== SQL-SERVER-STRATEGY-1 ==============

            #region ============== SQL-SERVER-STRATEGY-2 ==============

            //builder
            //.EnableDetailedErrors(true)
            //.EnableSensitiveDataLogging(true)
            //.UseLazyLoadingProxies(true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
            //.UseSqlServer(
            //    connectionString: configuration.GetConnectionString("ConnectionStrings"),
            //        sqlServerOptionsAction: optionsBuilder
            //            => optionsBuilder
            //            .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));

            #endregion ============== SQL-SERVER-STRATEGY-2 ==============
        });

        // Config for App User
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Lockout.AllowedForNewUsers = true; // Default true
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Default 5 ==> Lock trong bao lâu
            opt.Lockout.MaxFailedAccessAttempts = 3; // Default 5 ==> Sai bao nhiêu lần thì sẽ lock
        })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.AllowedForNewUsers = true; // Default true
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Default 5
            options.Lockout.MaxFailedAccessAttempts = 3; // Default 5

            // Config for password
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false; // Có chữ thường
            options.Password.RequireNonAlphanumeric = false; // Có số
            options.Password.RequireUppercase = false; // Có viết hoa
            options.Password.RequiredLength = 6; // Độ dài 6 kí tự
            options.Password.RequiredUniqueChars = 1; // Có 1 kí tự đạc biệt
            options.Lockout.AllowedForNewUsers = true;
        });
    }

    // DI for UnitOfWork and Repository
    public static void AddRepositoryBaseConfiguration(this IServiceCollection services)
    {
        services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
        services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));

    }

    public static OptionsBuilder<SqlServerRetryOptions> ConfigureSqlServerRetryOptions(this IServiceCollection services, IConfigurationSection section)
         => services
             .AddOptions<SqlServerRetryOptions>() // Add service options
             .Bind(section) // Bind section to IConfigurationSection
             .ValidateDataAnnotations() // Check Data Annotation meet the requirement in SqlServerRetryOptions
             .ValidateOnStart(); // Ensure, It is valid after start
}
