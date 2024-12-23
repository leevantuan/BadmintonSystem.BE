using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Price;

public static class Query
{
    public record GetPriceByIdQuery(Guid Id)
        : IQuery<Response.PriceDetailResponse>;

    public record GetPricesWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.PriceDetailResponse>>;
}
