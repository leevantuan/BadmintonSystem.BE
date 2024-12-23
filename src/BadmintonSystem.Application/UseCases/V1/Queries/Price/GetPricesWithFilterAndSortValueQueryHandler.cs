using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Price;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Price;

public sealed class GetPricesWithFilterAndSortValueQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Price, Guid> priceRepository)
    : IQueryHandler<Query.GetPricesWithFilterAndSortValueQuery, PagedResult<Response.PriceDetailResponse>>
{
    public async Task<Result<PagedResult<Response.PriceDetailResponse>>> Handle
        (Query.GetPricesWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        // Page Index and Page Size
        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Price>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Price>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Price>.UpperPageSize
                ? PagedResult<Domain.Entities.Price>.UpperPageSize
                : request.Data.PageSize;

        // Handle Query SQL
        var baseQueryBuilder = new StringBuilder();

        baseQueryBuilder.Append($@"SELECT * FROM ""{nameof(Domain.Entities.Price)}""
                             WHERE ""{nameof(Domain.Entities.Price.YardPrice)}""::TEXT ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = PriceExtension.GetSortPriceProperty(item.Key);
                baseQueryBuilder.Append(
                    $@"AND ""{nameof(Domain.Entities.Price)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

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
                    ? $@" ""{nameof(Domain.Entities.Price)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Price)}"".""{key}"" ASC, ");
            }

            baseQueryBuilder.Length -= 2;
        }

        baseQueryBuilder.Append($"\nOFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");


        List<Domain.Entities.Price> prices =
            await context.Price.FromSqlRaw(baseQueryBuilder.ToString()).ToListAsync(cancellationToken);

        int totalCount = prices.Count();

        var pricePagedResult =
            PagedResult<Domain.Entities.Price>.Create(prices, pageIndex, pageSize, totalCount);

        PagedResult<Response.PriceDetailResponse>? result =
            mapper.Map<PagedResult<Response.PriceDetailResponse>>(pricePagedResult);

        return Result.Success(result);
    }
}
