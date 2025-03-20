using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.API.DependencyInjection.Extensions;

public static class MigrationExtensions
{
    public static void AddMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using TenantDbContext tenantDbContext = scope.ServiceProvider.GetRequiredService<TenantDbContext>();

        tenantDbContext.Database.Migrate();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }
}
