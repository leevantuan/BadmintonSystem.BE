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

    public string? Code { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }

    public async Task<bool> SetTenant(string tenant)
    {
        var tenantInfo = await _context.Tenants.FirstOrDefaultAsync(x => x.Code == tenant);
        if (tenantInfo != null)
        {
            Code = tenantInfo.Code;
            Email = tenantInfo.Email;
            Name = tenantInfo.Name;
            ConnectionString = tenantInfo.ConnectionString;
            return true;
        }
        else
        {
            throw new Exception(" Tenant invalid");
        }
    }
}
