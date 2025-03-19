using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Infrastructure.Services;

public class CurrentTenantService : ICurrentTenantService
{
    private readonly TenantDbContext _context;

    public CurrentTenantService(TenantDbContext context)
    {
        _context = context;
    }

    public string? ConnectionString { get; set; }

    public string? Name { get; set; }

    public async Task<bool> SetTenant(string tenant)
    {
        var tenantInfo = await _context.Tenants.FirstOrDefaultAsync(x => x.Name == tenant);
        if (tenantInfo != null)
        {
            ConnectionString = tenantInfo.ConnectionString;
            return true;
        }
        else
        {
            throw new Exception(" Tenant invalid");
        }
    }
}
