using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.YardType;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.YardType;

public sealed class GetYardTypesWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context)
    : IQueryHandler<Query.GetYardTypesWithFilterAndSortValueQuery, PagedResult<Response.YardTypeDetailResponse>>
{
    public async Task<Result<PagedResult<Response.YardTypeDetailResponse>>> Handle
        (Query.GetYardTypesWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        int PageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.YardType>.DefaultPageIndex
            : request.Data.PageIndex;
        int PageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.YardType>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.YardType>.UpperPageSize
                ? PagedResult<Domain.Entities.YardType>.UpperPageSize
                : request.Data.PageSize;

        var yardTypesQuery = new StringBuilder();

        yardTypesQuery.Append($@"SELECT * FROM ""{nameof(Domain.Entities.YardType)}""
                             WHERE ""{nameof(Domain.Entities.YardType.Name)}"" ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = YardTypeExtension.GetSortYardTypeProperty(item.Key);
                yardTypesQuery.Append($@"AND ""{nameof(Domain.Entities.YardType)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    yardTypesQuery.Append($@"'%{value}%', ");
                }

                yardTypesQuery.Length -= 2;

                yardTypesQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            yardTypesQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                yardTypesQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.YardType)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.YardType)}"".""{key}"" ASC, ");
            }

            yardTypesQuery.Length -= 2;
        }

        yardTypesQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");


        List<Domain.Entities.YardType> yardTypes =
            await context.YardType.FromSqlRaw(yardTypesQuery.ToString()).ToListAsync(cancellationToken);

        int totalCount = yardTypes.Count();

        var yardTypePagedResult =
            PagedResult<Domain.Entities.YardType>.Create(yardTypes, PageIndex, PageSize, totalCount);

        PagedResult<Response.YardTypeDetailResponse>? result =
            mapper.Map<PagedResult<Response.YardTypeDetailResponse>>(yardTypePagedResult);

        return Result.Success(result);
    }
}
