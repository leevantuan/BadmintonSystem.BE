using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Tenant;

public class Query
{
    public record GetTenantsQuery()
    : IQuery<List<Response.TenantResponse>>;

    public record GetTenantByIdQuery(Guid tenantId)
    : IQuery<Response.TenantResponse>;
}
