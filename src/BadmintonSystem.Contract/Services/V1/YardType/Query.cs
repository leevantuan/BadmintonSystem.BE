using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.YardType;

public static class Query
{
    public record GetYardTypeByIdQuery(Guid Id)
        : IQuery<Response.YardTypeResponse>;

    public record GetYardTypesWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.YardTypeDetailResponse>>;
}
