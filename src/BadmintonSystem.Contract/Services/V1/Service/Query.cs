using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Service;

public static class Query
{
    public record GetServicesQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.ServiceResponse>>;

    public record GetServiceByIdQuery(Guid Id)
        : IQuery<Response.ServiceResponse>;

    public record GetServicesWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.ServiceDetailResponse>>;
}
