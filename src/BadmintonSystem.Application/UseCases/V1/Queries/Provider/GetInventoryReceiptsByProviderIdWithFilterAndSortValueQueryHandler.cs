using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Provider;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Provider;

public sealed class GetInventoryReceiptsByProviderIdWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.InventoryReceipt, Guid> inventoryReceiptRepository)
    : IQueryHandler<Query.GetInventoryReceiptsByProviderIdWithFilterAndSortValueQuery,
        PagedResult<Response.GetInventoryReceiptByProviderIdResponse>>
{
    public async Task<Result<PagedResult<Response.GetInventoryReceiptByProviderIdResponse>>> Handle
        (Query.GetInventoryReceiptsByProviderIdWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Provider provider =
            await context.Provider.FirstOrDefaultAsync(x => x.Id == request.ProviderId, cancellationToken)
            ?? throw new ProviderException.ProviderNotFoundException(
                request.ProviderId);

        Response.ProviderResponse? providerResponse = mapper.Map<Response.ProviderResponse>(provider);

        string serviceColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Service,
                Contract.Services.V1.Service.Response.ServiceResponse>();

        string inventoryReceiptColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.InventoryReceipt,
                Contract.Services.V1.InventoryReceipt.Response.InventoryReceiptResponse>();

        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Provider>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Provider>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Provider>.UpperPageSize
                ? PagedResult<Domain.Entities.Provider>.UpperPageSize
                : request.Data.PageSize;

        var inventoryReceiptQueryBuilder = new StringBuilder();

        inventoryReceiptQueryBuilder.Append($@"SELECT {serviceColumns}, {inventoryReceiptColumns}
            FROM ""{nameof(Domain.Entities.InventoryReceipt)}"" AS inventoryReceipt
            JOIN ""{nameof(Domain.Entities.Service)}"" AS service
            ON service.""{nameof(Domain.Entities.Service.Id)}"" = inventoryReceipt.""{nameof(Domain.Entities.InventoryReceipt.ServiceId)}""
            AND service.""{nameof(Domain.Entities.Service.IsDeleted)}"" = false
            WHERE inventoryReceipt.""{nameof(Domain.Entities.InventoryReceipt.ProviderId)}"" = '{request.ProviderId}'
            AND inventoryReceipt.""{nameof(Domain.Entities.InventoryReceipt.IsDeleted)}"" = false");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = InventoryReceiptExtension.GetSortInventoryReceiptProperty(item.Key);
                inventoryReceiptQueryBuilder.Append(
                    $@"AND inventoryReceipt.""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    inventoryReceiptQueryBuilder.Append($@"'%{value}%', ");
                }

                inventoryReceiptQueryBuilder.Length -= 2;

                inventoryReceiptQueryBuilder.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            inventoryReceiptQueryBuilder.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = InventoryReceiptExtension.GetSortInventoryReceiptProperty(item.Key);
                inventoryReceiptQueryBuilder.Append(item.Value == SortOrder.Descending
                    ? $@" inventoryReceipt.""{key}"" DESC, "
                    : $@" inventoryReceipt.""{key}"" ASC, ");
            }

            inventoryReceiptQueryBuilder.Length -= 2;
        }

        inventoryReceiptQueryBuilder.Append(
            $"\nOFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

        List<Response.GetInventoryReceiptByProviderIdSql> inventoryReceipts = await inventoryReceiptRepository
            .ExecuteSqlQuery<Response.GetInventoryReceiptByProviderIdSql>(
                FormattableStringFactory.Create(inventoryReceiptQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        // Group by
        var results = inventoryReceipts.GroupBy(p => p.InventoryReceipt_Id)
            .Select(g => new Response.GetInventoryReceiptByProviderIdResponse
            {
                Provider = providerResponse,
                InventoryReceipts = g.Select(x => new Response.GetInventoryReceiptResponse
                {
                    InventoryReceipt = g.Select(x =>
                        new Contract.Services.V1.InventoryReceipt.Response.InventoryReceiptResponse
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
                    }).FirstOrDefault()
                }).Distinct().ToList()
            })
            .ToList();

        var inventoryReceiptPagedResult =
            PagedResult<Response.GetInventoryReceiptByProviderIdResponse>.Create(
                results,
                pageIndex,
                pageSize,
                results.Count());

        return Result.Success(inventoryReceiptPagedResult);
    }
}
