using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Sale;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Sale;

public sealed class GetSalesWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context)
    : IQueryHandler<Query.GetSalesWithFilterAndSortValueQuery, PagedResult<Response.SaleDetailResponse>>
{
    public async Task<Result<PagedResult<Response.SaleDetailResponse>>> Handle
        (Query.GetSalesWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        int PageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Sale>.DefaultPageIndex
            : request.Data.PageIndex;
        int PageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Sale>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Sale>.UpperPageSize
                ? PagedResult<Domain.Entities.Sale>.UpperPageSize
                : request.Data.PageSize;

        var salesQuery = new StringBuilder();

        salesQuery.Append($@"SELECT * FROM ""{nameof(Domain.Entities.Sale)}""
                             WHERE ""{nameof(Domain.Entities.Sale.Name)}"" ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = SaleExtension.GetSortSaleProperty(item.Key);
                salesQuery.Append($@"AND ""{nameof(Domain.Entities.Sale)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    salesQuery.Append($@"'%{value}%', ");
                }

                salesQuery.Length -= 2;

                salesQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            salesQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                salesQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.Sale)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Sale)}"".""{key}"" ASC, ");
            }

            salesQuery.Length -= 2;
        }

        salesQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");

        List<Domain.Entities.Sale> sales =
            await context.Sale.FromSqlRaw(salesQuery.ToString()).ToListAsync(cancellationToken);

        int totalCount = sales.Count();

        var salePagedResult = PagedResult<Domain.Entities.Sale>.Create(sales, PageIndex, PageSize, totalCount);

        PagedResult<Response.SaleDetailResponse>? result =
            mapper.Map<PagedResult<Response.SaleDetailResponse>>(salePagedResult);

        return Result.Success(result);
    }
}
