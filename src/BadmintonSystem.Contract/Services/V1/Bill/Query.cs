using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Bill;

public static class Query
{
    public record GetBillByIdQuery(Guid Id)
        : IQuery<Response.BillResponse>;

    public record GetBillsWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.BillDetailResponse>>;
}
