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
        var categoriesQuery = new StringBuilder();

        categoriesQuery.Append($@"SELECT * FROM ""{nameof(Domain.Entities.Category)}""
                             WHERE ""{nameof(Domain.Entities.Category.Name)}"" ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = CategoryExtension.GetSortCategoryProperty(item.Key);
                categoriesQuery.Append(
                    $@"AND ""{nameof(Domain.Entities.Category)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    categoriesQuery.Append($@"'%{value}%', ");
                }

                categoriesQuery.Length -= 2;

                categoriesQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            categoriesQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                categoriesQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.Category)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Category)}"".""{key}"" ASC, ");
            }

            categoriesQuery.Length -= 2;
        }

        categoriesQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");


        List<Domain.Entities.Category> categories =
            await context.Category.FromSqlRaw(categoriesQuery.ToString()).ToListAsync(cancellationToken);

        int totalCount = categories.Count();

        var categoryPagedResult =
            PagedResult<Domain.Entities.Category>.Create(categories, PageIndex, PageSize, totalCount);

        PagedResult<Response.CategoryDetailResponse>? result =
            mapper.Map<PagedResult<Response.CategoryDetailResponse>>(categoryPagedResult);

        return Result.Success(result);
    }
}
