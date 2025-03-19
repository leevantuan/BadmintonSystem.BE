using BadmintonSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Persistence;

public sealed class TenantDbContext : DbContext
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants { get; set; }
}
