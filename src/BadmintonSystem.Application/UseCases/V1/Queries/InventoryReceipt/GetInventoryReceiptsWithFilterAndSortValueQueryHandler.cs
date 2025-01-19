using System.Runtime.CompilerServices;
using System.Text;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.InventoryReceipt;
using BadmintonSystem.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.InventoryReceipt;

public sealed class GetInventoryReceiptsWithFilterAndSortValueQueryHandler(
    IRepositoryBase<Domain.Entities.InventoryReceipt, Guid> inventoryReceiptRepository)
    : IQueryHandler<Query.GetInventoryReceiptsWithFilterAndSortValueQuery,
        PagedResult<Response.InventoryReceiptDetailResponse>>
{
    public async Task<Result<PagedResult<Response.InventoryReceiptDetailResponse>>> Handle
        (Query.GetInventoryReceiptsWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        string serviceColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Service,
                Contract.Services.V1.Service.Response.ServiceResponse>();

        string inventoryReceiptColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.InventoryReceipt,
                Response.InventoryReceiptDetail>();

        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.InventoryReceipt>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.InventoryReceipt>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.InventoryReceipt>.UpperPageSize
                ? PagedResult<Domain.Entities.InventoryReceipt>.UpperPageSize
                : request.Data.PageSize;

        var baseQueryBuilder = new StringBuilder();

        baseQueryBuilder.Append(
            $@"FROM ""{nameof(Domain.Entities.InventoryReceipt)}"" AS inventoryReceipt
                JOIN ""{nameof(Domain.Entities.Service)}"" AS service
                ON service.""{nameof(Domain.Entities.Service.Id)}"" = inventoryReceipt.""{nameof(Domain.Entities.InventoryReceipt.ServiceId)}""
                AND service.""{nameof(Domain.Entities.Service.IsDeleted)}"" = false
                WHERE inventoryReceipt.""{nameof(Domain.Entities.InventoryReceipt.IsDeleted)}"" = false
                AND inventoryReceipt.""{nameof(Domain.Entities.InventoryReceipt.Unit)}"" ILIKE '%{request.Data.SearchTerm}%'");

        baseQueryBuilder.Append(
            $@"AND inventoryReceipt.""{nameof(Domain.Entities.InventoryReceipt.CreatedDate)}""::DATE BETWEEN '{request.Filter.StartDate.Date}' AND '{request.Filter.EndDate.Date}' ");

        if (request.Filter.ProviderIds.Any())
        {
            baseQueryBuilder.Append(
                $@"AND inventoryReceipt.""{nameof(Domain.Entities.InventoryReceipt.ProviderId)}""::TEXT ILIKE ANY (ARRAY[");

            foreach (Guid value in request.Filter.ProviderIds)
            {
                baseQueryBuilder.Append($@"'%{value.ToString()}%', ");
            }

            baseQueryBuilder.Length -= 2;

            baseQueryBuilder.Append("]) ");
        }

        if (request.Filter.ServiceIds.Any())
        {
            baseQueryBuilder.Append(
                $@"AND inventoryReceipt.""{nameof(Domain.Entities.InventoryReceipt.ServiceId)}""::TEXT ILIKE ANY (ARRAY[");

            foreach (Guid value in request.Filter.ServiceIds)
            {
                baseQueryBuilder.Append($@"'%{value.ToString()}%', ");
            }

            baseQueryBuilder.Length -= 2;

            baseQueryBuilder.Append("]) ");
        }

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = InventoryReceiptExtension.GetSortInventoryReceiptProperty(item.Key);
                baseQueryBuilder.Append(
                    $@"AND inventoryReceipt.""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    baseQueryBuilder.Append($@"'%{value}%', ");
                }

                baseQueryBuilder.Length -= 2;

                baseQueryBuilder.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            baseQueryBuilder.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                baseQueryBuilder.Append(item.Value == SortOrder.Descending
                    ? $@" inventoryReceipt.""{key}"" DESC, "
                    : $@" inventoryReceipt.""{key}"" ASC, ");
            }

            baseQueryBuilder.Length -= 2;
        }

        int totalCount = await TotalCount(baseQueryBuilder.ToString(), cancellationToken);

        baseQueryBuilder.Append($"\nOFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

        var totalPriceQueryBuilder = new StringBuilder();
        totalPriceQueryBuilder.Append(
            $@"SELECT SUM(inventoryReceipt.""{nameof(Domain.Entities.InventoryReceipt.Price)}"") AS ""{nameof(SqlResponse.TotalPriceSqlResponse.TotalPrice)}"" ");
        totalPriceQueryBuilder.Append(baseQueryBuilder);

        SqlResponse.TotalPriceSqlResponse totalPrice = await inventoryReceiptRepository
            .ExecuteSqlQuery<SqlResponse.TotalPriceSqlResponse>(
                FormattableStringFactory.Create(totalPriceQueryBuilder.ToString()))
            .FirstAsync(cancellationToken);

        var inventoryReceiptQueryBuilder = new StringBuilder();
        inventoryReceiptQueryBuilder.Append($@"SELECT {serviceColumns}, {inventoryReceiptColumns}");
        inventoryReceiptQueryBuilder.Append(baseQueryBuilder);

        List<Response.InventoryReceiptDetailSql> inventoryReceipts = await inventoryReceiptRepository
            .ExecuteSqlQuery<Response.InventoryReceiptDetailSql>(
                FormattableStringFactory.Create(inventoryReceiptQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        var inventoryReceiptPagedResult =
            PagedResult<Response.InventoryReceiptDetailResponse>.Create(
                GroupByData(inventoryReceipts, totalPrice),
                pageIndex,
                pageSize,
                totalCount);

        return Result.Success(inventoryReceiptPagedResult);
    }

    private async Task<int> TotalCount(string baseQuery, CancellationToken cancellationToken)
    {
        var countQueryBuilder = new StringBuilder();
        countQueryBuilder.Append(
            $@"SELECT COUNT(*) AS ""{nameof(SqlResponse.TotalCountSqlResponse.TotalCount)}""");
        countQueryBuilder.Append(" \n");

        countQueryBuilder.Append(baseQuery);
        SqlResponse.TotalCountSqlResponse totalCountQueryResult = await inventoryReceiptRepository
            .ExecuteSqlQuery<SqlResponse.TotalCountSqlResponse>(
                FormattableStringFactory.Create(countQueryBuilder.ToString()))
            .SingleAsync(cancellationToken);

        return totalCountQueryResult.TotalCount;
    }

    private List<Response.InventoryReceiptDetailResponse> GroupByData
        (List<Response.InventoryReceiptDetailSql> inventoryReceipts, SqlResponse.TotalPriceSqlResponse totalPrice)
    {
        var results = inventoryReceipts.GroupBy(p => p.InventoryReceipt_Id)
            .Select(g => new Response.InventoryReceiptDetailResponse
            {
                InventoryReceipt = g.Select(x =>
                    new Response.InventoryReceiptResponse
                    {
                        Id = x.InventoryReceipt_Id ?? Guid.Empty,
                        Quantity = x.InventoryReceipt_Quantity,
                        Unit = x.InventoryReceipt_Unit,
                        Price = x.InventoryReceipt_Price,
                        ServiceId = x.InventoryReceipt_ServiceId,
                        ProviderId = x.InventoryReceipt_ProviderId
                    }).FirstOrDefault(),

                Service = g.Select(x => new Contract.Services.V1.Service.Response.ServiceResponse
                {
                    Id = x.Service_Id ?? Guid.Empty,
                    Name = x.Service_Name ?? string.Empty,
                    PurchasePrice = x.Service_PurchasePrice ?? 0,
                    SellingPrice = x.Service_SellingPrice ?? 0,
                    QuantityInStock = x.Service_QuantityInStock ?? 0,
                    Unit = x.Service_Unit ?? string.Empty,
                    CategoryId = x.Service_CategoryId ?? Guid.Empty,
                    QuantityPrinciple = x.Service_QuantityPrinciple ?? 0,
                    OriginalQuantityId = x.Service_OriginalQuantityId ?? Guid.Empty
                }).FirstOrDefault(),

                Id = g.Key ?? Guid.Empty,
                CreatedDate = g.First().InventoryReceipt_CreatedDate ?? DateTime.Now,
                CreatedBy = g.First().InventoryReceipt_CreatedBy ?? Guid.Empty,
                ModifiedDate = g.First().InventoryReceipt_ModifiedDate ?? DateTime.Now,
                ModifiedBy = g.First().InventoryReceipt_ModifiedBy ?? Guid.Empty,
                IsDeleted = g.First().InventoryReceipt_IsDeleted ?? false,
                DeletedAt = g.First().InventoryReceipt_DeletedAt,
                TotalPrice = totalPrice.TotalPrice
            }).Distinct().ToList();

        return results;
    }
}
