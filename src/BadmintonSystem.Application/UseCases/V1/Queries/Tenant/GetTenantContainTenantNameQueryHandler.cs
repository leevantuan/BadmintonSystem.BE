using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Tenant;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Tenant;

public sealed class GetTenantContainTenantNameQueryHandler(
    TenantDbContext context,
    IMapper mapper)
    : IQueryHandler<Query.GetTenantContainTenantNameQuery, Response.TenantResponse>
{
    public async Task<Result<Response.TenantResponse>> Handle(Query.GetTenantContainTenantNameQuery request, CancellationToken cancellationToken)
    {
        var tenant = await context.Tenants.FirstOrDefaultAsync(x => x.Name.Trim().ToLower().Contains(request.tenantName.Trim().ToLower()), cancellationToken)
            ?? throw new ApplicationException($"Tenant name invalid");

        var result = mapper.Map<Response.TenantResponse>(tenant);

        return Result.Success(result);
    }
}
