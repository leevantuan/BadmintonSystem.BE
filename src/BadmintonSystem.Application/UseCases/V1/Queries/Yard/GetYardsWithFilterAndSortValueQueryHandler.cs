using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Yard;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Yard;

public sealed class GetYardsWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context)
    : IQueryHandler<Query.GetYardsWithFilterAndSortValueQuery, PagedResult<Response.YardDetailResponse>>
{
    public async Task<Result<PagedResult<Response.YardDetailResponse>>> Handle
        (Query.GetYardsWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        int PageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Yard>.DefaultPageIndex
            : request.Data.PageIndex;
        int PageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Yard>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Yard>.UpperPageSize
                ? PagedResult<Domain.Entities.Yard>.UpperPageSize
                : request.Data.PageSize;

        var yardsQuery = new StringBuilder();

        yardsQuery.Append($@"SELECT * FROM ""{nameof(Domain.Entities.Yard)}""
                             WHERE ""{nameof(Domain.Entities.Yard.Name)}"" ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = YardExtension.GetSortYardProperty(item.Key);
                yardsQuery.Append($@"AND ""{nameof(Domain.Entities.Yard)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    yardsQuery.Append($@"'%{value}%', ");
                }

                yardsQuery.Length -= 2;

                yardsQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            yardsQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                yardsQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.Yard)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Yard)}"".""{key}"" ASC, ");
            }

            yardsQuery.Length -= 2;
        }

        yardsQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");

        List<Domain.Entities.Yard> yards =
            await context.Yard.FromSqlRaw(yardsQuery.ToString()).ToListAsync(cancellationToken);

        int totalCount = yards.Count();

        var yardPagedResult = PagedResult<Domain.Entities.Yard>.Create(yards, PageIndex, PageSize, totalCount);

        PagedResult<Response.YardDetailResponse>? result =
            mapper.Map<PagedResult<Response.YardDetailResponse>>(yardPagedResult);

        return Result.Success(result);
    }
}
