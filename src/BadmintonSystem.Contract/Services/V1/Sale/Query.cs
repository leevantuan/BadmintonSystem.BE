using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Sale;

public static class Query
{
    public record GetSalesQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.SaleResponse>>;

    public record GetSaleByIdQuery(Guid Id)
        : IQuery<Response.SaleResponse>;

    public record GetSalesWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.SaleDetailResponse>>;
}
