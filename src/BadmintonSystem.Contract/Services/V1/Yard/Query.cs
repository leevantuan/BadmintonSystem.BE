using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Yard;

public static class Query
{
    public record GetYardByIdQuery(Guid Id)
        : IQuery<Response.YardResponse>;

    public record GetYardsWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.YardDetailResponse>>;
}
