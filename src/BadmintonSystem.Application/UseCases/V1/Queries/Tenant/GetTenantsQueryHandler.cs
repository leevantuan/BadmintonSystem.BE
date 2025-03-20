using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Tenant;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Tenant;

public sealed class GetTenantsQueryHandler(
    TenantDbContext context,
    IMapper mapper)
    : IQueryHandler<Query.GetTenantsQuery, List<Response.TenantResponse>>
{
    public async Task<Result<List<Response.TenantResponse>>> Handle(Query.GetTenantsQuery request, CancellationToken cancellationToken)
    {
        var tenants = await context.Tenants.ToListAsync(cancellationToken);

        var result = mapper.Map<List<Response.TenantResponse>>(tenants);

        return Result.Success(result);
    }
}
