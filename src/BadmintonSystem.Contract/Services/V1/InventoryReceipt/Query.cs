using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.InventoryReceipt;

public static class Query
{
    public record GetInventoryReceiptByIdQuery(Guid Id)
        : IQuery<Response.InventoryReceiptDetailResponse>;

    public record GetInventoryReceiptsWithFilterAndSortValueQuery(
        Request.FilterInventoryReceiptRequest Filter,
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.InventoryReceiptDetailResponse>>;
}
