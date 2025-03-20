using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Tenant;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Tenant;

public sealed class GetTenantByIdQueryHandler(
    TenantDbContext context,
    IMapper mapper)
    : IQueryHandler<Query.GetTenantByIdQuery, Response.TenantResponse>
{
    public async Task<Result<Response.TenantResponse>> Handle(Query.GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var tenant = await context.Tenants.FirstOrDefaultAsync(x => x.Id == request.tenantId, cancellationToken)
            ?? throw new ApplicationException($"Tenant invalid");

        var result = mapper.Map<Response.TenantResponse>(tenant);

        return Result.Success(result);
    }
}
