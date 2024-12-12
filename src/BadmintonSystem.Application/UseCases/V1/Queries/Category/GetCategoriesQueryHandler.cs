using System.Linq.Expressions;
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

public sealed class GetCategoriesQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    : IQueryHandler<Query.GetCategoriesQuery, PagedResult<Response.CategoryResponse>>
{
    public async Task<Result<PagedResult<Response.CategoryResponse>>> Handle
        (Query.GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        if (request.Data.SortColumnAndOrder.Any())
        {
            int PageIndex = request.Data.PageIndex <= 0
                ? PagedResult<Domain.Entities.Category>.DefaultPageIndex
                : request.Data.PageIndex;
            int PageSize = request.Data.PageSize <= 0
                ? PagedResult<Domain.Entities.Category>.DefaultPageSize
                : request.Data.PageSize > PagedResult<Domain.Entities.Category>.UpperPageSize
                    ? PagedResult<Domain.Entities.Category>.UpperPageSize
                    : request.Data.PageSize;

            string categorysQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? $@"SELECT * FROM ""{nameof(Domain.Entities.Category)}"" ORDER BY "
                : $@"SELECT * FROM ""{nameof(Domain.Entities.Category)}""
                              WHERE ""{nameof(Domain.Entities.Category.Name)}"" LIKE '%{request.Data.SearchTerm}%' 
                              ORDER BY ";

            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = CategoryExtension.GetSortCategoryProperty(item.Key);

                categorysQuery += item.Value == SortOrder.Descending
                    ? $"\"{key}\" DESC, "
                    : $"\"{key}\" ASC, ";
            }

            categorysQuery = categorysQuery.Remove(categorysQuery.Length - 2);

            categorysQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

            List<Domain.Entities.Category> categorys =
                await context.Category.FromSqlRaw(categorysQuery).ToListAsync(cancellationToken);

            int totalCount = await context.Category.CountAsync(cancellationToken);

            var categoryPagedResult =
                PagedResult<Domain.Entities.Category>.Create(categorys, PageIndex, PageSize, totalCount);

            PagedResult<Response.CategoryResponse>? result =
                mapper.Map<PagedResult<Response.CategoryResponse>>(categoryPagedResult);

            return Result.Success(result);
        }
        else
        {
            IQueryable<Domain.Entities.Category> categorysQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? categoryRepository.FindAll()
                : categoryRepository.FindAll(x => x.Name.Contains(request.Data.SearchTerm));

            categorysQuery = request.Data.SortOrder == SortOrder.Descending
                ? categorysQuery.OrderByDescending(GetSortColumnProperty(request))
                : categorysQuery.OrderBy(GetSortColumnProperty(request));

            var categorys = await PagedResult<Domain.Entities.Category>.CreateAsync(categorysQuery,
                request.Data.PageIndex, request.Data.PageSize);

            PagedResult<Response.CategoryResponse>? result =
                mapper.Map<PagedResult<Response.CategoryResponse>>(categorys);

            return Result.Success(result);
        }
    }

    private static Expression<Func<Domain.Entities.Category, object>> GetSortColumnProperty
        (Query.GetCategoriesQuery request)
    {
        return request.Data.SortColumn?.Trim().ToLower() switch
        {
            "name" => category => category.Name,
            _ => category => category.Id
        };
    }
}
