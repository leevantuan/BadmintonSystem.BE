using System.Text;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Bill;

public sealed class GetBillsWithFilterAndSortValueQueryHandler(
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Bill, Guid> billRepository)
    : IQueryHandler<Query.GetBillsWithFilterAndSortValueQuery, PagedResult<Response.BillDetailResponse>>
{
    public Task<Result<PagedResult<Response.BillDetailResponse>>> Handle
        (Query.GetBillsWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        // Page Index and Page Size
        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Bill>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Bill>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Bill>.UpperPageSize
                ? PagedResult<Domain.Entities.Bill>.UpperPageSize
                : request.Data.PageSize;

        var baseQueryBuilder = new StringBuilder();

        throw new Exception();
    }
}
