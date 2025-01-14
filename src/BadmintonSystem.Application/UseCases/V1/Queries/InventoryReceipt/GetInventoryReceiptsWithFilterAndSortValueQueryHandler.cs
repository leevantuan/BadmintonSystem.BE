using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.InventoryReceipt;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.InventoryReceipt;

public sealed class GetInventoryReceiptsWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.InventoryReceipt, Guid> inventoryReceiptRepository)
    : IQueryHandler<Query.GetInventoryReceiptsWithFilterAndSortValueQuery,
        PagedResult<Response.InventoryReceiptDetailResponse>>
{
    public async Task<Result<PagedResult<Response.InventoryReceiptDetailResponse>>> Handle
        (Query.GetInventoryReceiptsWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        // Page Index and Page Size
        int PageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.InventoryReceipt>.DefaultPageIndex
            : request.Data.PageIndex;
        int PageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.InventoryReceipt>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.InventoryReceipt>.UpperPageSize
                ? PagedResult<Domain.Entities.InventoryReceipt>.UpperPageSize
                : request.Data.PageSize;

        // Handle Query SQL
        var inventoryReceiptsQuery = new StringBuilder();

        inventoryReceiptsQuery.Append($@"SELECT * FROM ""{nameof(Domain.Entities.InventoryReceipt)}""
                             WHERE ""{nameof(Domain.Entities.InventoryReceipt.Unit)}"" ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = InventoryReceiptExtension.GetSortInventoryReceiptProperty(item.Key);
                inventoryReceiptsQuery.Append(
                    $@"AND ""{nameof(Domain.Entities.InventoryReceipt)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    inventoryReceiptsQuery.Append($@"'%{value}%', ");
                }

                inventoryReceiptsQuery.Length -= 2;

                inventoryReceiptsQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            inventoryReceiptsQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                inventoryReceiptsQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.InventoryReceipt)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.InventoryReceipt)}"".""{key}"" ASC, ");
            }

            inventoryReceiptsQuery.Length -= 2;
        }

        inventoryReceiptsQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");


        List<Domain.Entities.InventoryReceipt> inventoryReceipts =
            await context.InventoryReceipt.FromSqlRaw(inventoryReceiptsQuery.ToString()).ToListAsync(cancellationToken);

        int totalCount = inventoryReceipts.Count();

        var inventoryReceiptPagedResult =
            PagedResult<Domain.Entities.InventoryReceipt>.Create(inventoryReceipts, PageIndex, PageSize, totalCount);

        PagedResult<Response.InventoryReceiptDetailResponse>? result =
            mapper.Map<PagedResult<Response.InventoryReceiptDetailResponse>>(inventoryReceiptPagedResult);

        return Result.Success(result);
    }
}
