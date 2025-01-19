using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Service;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Service;

public sealed class GetInventoryReceiptsByServiceIdWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Service, Guid> serviceRepository)
    : IQueryHandler<Query.GetInventoryReceiptsByServiceIdWithFilterAndSortValueQuery,
        PagedResult<Response.GetInventoryReceiptByServiceIdResponse>>
{
    public async Task<Result<PagedResult<Response.GetInventoryReceiptByServiceIdResponse>>> Handle
        (Query.GetInventoryReceiptsByServiceIdWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Service service = await serviceRepository.FindByIdAsync(request.ServiceId, cancellationToken)
                                          ?? throw new ServiceException.ServiceNotFoundException(request.ServiceId);

        Response.ServiceResponse? serviceResponse = mapper.Map<Response.ServiceResponse>(service);

        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Service>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Service>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Service>.UpperPageSize
                ? PagedResult<Domain.Entities.Service>.UpperPageSize
                : request.Data.PageSize;

        string providerColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Provider,
                Contract.Services.V1.Provider.Response.ProviderResponse>();

        string inventoryReceiptColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.InventoryReceipt,
                Contract.Services.V1.InventoryReceipt.Response.InventoryReceiptResponse>();

        var inventoryReceiptQueryBuilder = new StringBuilder();

        inventoryReceiptQueryBuilder.Append($@"SELECT {providerColumns}, {inventoryReceiptColumns}
            FROM ""{nameof(Domain.Entities.InventoryReceipt)}"" AS inventoryReceipt
            JOIN ""{nameof(Domain.Entities.Provider)}"" AS provider
            ON provider.""{nameof(Domain.Entities.Provider.Id)}"" = inventoryReceipt.""{nameof(Domain.Entities.InventoryReceipt.ProviderId)}""
            AND provider.""{nameof(Domain.Entities.Provider.IsDeleted)}"" = false
            WHERE inventoryReceipt.""{nameof(Domain.Entities.InventoryReceipt.ServiceId)}"" = '{request.ServiceId}'
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

        List<Response.GetInventoryReceiptByServiceIdSql> inventoryReceipts = await serviceRepository
            .ExecuteSqlQuery<Response.GetInventoryReceiptByServiceIdSql>(
                FormattableStringFactory.Create(inventoryReceiptQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        // Group by
        var results = inventoryReceipts.GroupBy(p => p.InventoryReceipt_Id)
            .Select(g => new Response.GetInventoryReceiptByServiceIdResponse
            {
                Service = serviceResponse,
                InventoryReceipts = g.Select(x => new Response.GetInventoryReceiptDetail
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

                    Provider = g.Select(x => new Contract.Services.V1.Provider.Response.ProviderResponse
                    {
                        Id = x.Provider_Id ?? Guid.Empty,
                        Name = x.Provider_Name ?? string.Empty,
                        PhoneNumber = x.Provider_PhoneNumber ?? string.Empty,
                        Address = x.Provider_Address ?? string.Empty
                    }).FirstOrDefault()
                }).Distinct().ToList()
            })
            .ToList();

        var inventoryReceiptPagedResult =
            PagedResult<Response.GetInventoryReceiptByServiceIdResponse>.Create(
                results,
                pageIndex,
                pageSize,
                results.Count());

        return Result.Success(inventoryReceiptPagedResult);
    }
}
