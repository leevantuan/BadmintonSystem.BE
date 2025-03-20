using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.API.DependencyInjection.Extensions;

public static class MigrationExtensions
{
    public static void AddMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using TenantDbContext tenantDbContext = scope.ServiceProvider.GetRequiredService<TenantDbContext>();

        if (tenantDbContext.Database.GetPendingMigrations().Any() || !tenantDbContext.Database.CanConnect())
        {
            tenantDbContext.Database.Migrate();
        }

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (dbContext.Database.GetPendingMigrations().Any() || !dbContext.Database.CanConnect())
        {
            dbContext.Database.Migrate();
        }
    }
}
