namespace BadmintonSystem.Contract.Abstractions.Services;

public interface ICurrentTenantService
{
    string? ConnectionString { get; set; }

    string? Name { get; set; }

    Task<bool> SetTenant(string tenant);
}
