using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Category;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Category;

public sealed class GetCategoriesWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    : IQueryHandler<Query.GetCategoriesWithFilterAndSortValueQuery, PagedResult<Response.CategoryDetailResponse>>
{
    public async Task<Result<PagedResult<Response.CategoryDetailResponse>>> Handle
        (Query.GetCategoriesWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        // Page Index and Page Size
        int PageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Category>.DefaultPageIndex
            : request.Data.PageIndex;
        int PageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Category>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Category>.UpperPageSize
                ? PagedResult<Domain.Entities.Category>.UpperPageSize
                : request.Data.PageSize;

        // Handle Query SQL
        var baseQuery = new StringBuilder();
        baseQuery.Append($@"FROM ""{nameof(Domain.Entities.Category)}""
                            WHERE ""{nameof(Domain.Entities.Category.Name)}"" ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = CategoryExtension.GetSortCategoryProperty(item.Key);
                baseQuery.Append(
                    $@"AND ""{nameof(Domain.Entities.Category)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    baseQuery.Append($@"'%{value}%', ");
                }

                baseQuery.Length -= 2;

                baseQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            baseQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                baseQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.Category)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Category)}"".""{key}"" ASC, ");
            }

            baseQuery.Length -= 2;
        }

        int totalCount = await TotalCount(baseQuery.ToString(), cancellationToken);

        var categoriesQuery = new StringBuilder();
        categoriesQuery.Append(@"SELECT * ");
        categoriesQuery.Append(baseQuery);
        categoriesQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");


        List<Domain.Entities.Category> categories =
            await context.Category.FromSqlRaw(categoriesQuery.ToString()).ToListAsync(cancellationToken);

        var categoryPagedResult =
            PagedResult<Domain.Entities.Category>.Create(categories, PageIndex, PageSize, totalCount);

        PagedResult<Response.CategoryDetailResponse>? result =
            mapper.Map<PagedResult<Response.CategoryDetailResponse>>(categoryPagedResult);

        return Result.Success(result);
    }

    private async Task<int> TotalCount(string baseQuery, CancellationToken cancellationToken)
    {
        var countQueryBuilder = new StringBuilder();
        countQueryBuilder.Append(
            $@"SELECT COUNT(*) AS ""{nameof(SqlResponse.TotalCountSqlResponse.TotalCount)}""");
        countQueryBuilder.Append(" \n");

        countQueryBuilder.Append(baseQuery);
        SqlResponse.TotalCountSqlResponse totalCountQueryResult = await categoryRepository
            .ExecuteSqlQuery<SqlResponse.TotalCountSqlResponse>(
                FormattableStringFactory.Create(countQueryBuilder.ToString()))
            .SingleAsync(cancellationToken);

        return totalCountQueryResult.TotalCount;
    }
}
