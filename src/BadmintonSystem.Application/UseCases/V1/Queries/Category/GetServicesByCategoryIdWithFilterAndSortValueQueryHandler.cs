using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Category;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Category;

public sealed class GetServicesByCategoryIdWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    : IQueryHandler<Query.GetServicesByCategoryIdWithFilterAndSortValueQuery,
        PagedResult<Response.GetServicesByCategoryIdResponse>>
{
    public async Task<Result<PagedResult<Response.GetServicesByCategoryIdResponse>>> Handle
        (Query.GetServicesByCategoryIdWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Category>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Category>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Category>.UpperPageSize
                ? PagedResult<Domain.Entities.Category>.UpperPageSize
                : request.Data.PageSize;

        // SQL Query String
        string categoryColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Category, Response.CategoryResponse>();
        string serviceColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Service,
                Contract.Services.V1.Service.Response.ServiceResponse>();

        var categoriesQueryBuilder = new StringBuilder();
        categoriesQueryBuilder.Append(
            $@"SELECT {categoryColumns}, {serviceColumns}
               FROM ""{nameof(Domain.Entities.Category)}"" AS category
               LEFT JOIN ""{nameof(Domain.Entities.Service)}"" AS service
               ON category.""{nameof(Domain.Entities.Category.Id)}"" = service.""{nameof(Domain.Entities.Service.CategoryId)}""
               WHERE category.""{nameof(Domain.Entities.Category.Id)}"" = '{request.CategoryId}'");

        categoriesQueryBuilder.Append($"\nOFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

        // SQL RAW
        List<Response.CategorySqlResponse> queryResult = await categoryRepository
            .ExecuteSqlQuery<Response.CategorySqlResponse>(
                FormattableStringFactory.Create(categoriesQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);
        var categories = queryResult.GroupBy(r => r.Category_Id)
            .Select(g => new Response.GetServicesByCategoryIdResponse
            {
                Id = g.Key,
                Name = g.First().Category_Name,
                Services = g.Where(s => s.Service_Id != null)
                    .Select(s =>
                        new Contract.Services.V1.Service.Response.ServiceDetailResponse
                        {
                            Id = s.Service_Id ?? Guid.Empty,
                            Name = s.Service_Name ?? string.Empty,
                            SellingPrice = s.Service_SellingPrice ?? 0,
                            PurchasePrice = s.Service_PurchasePrice ?? 0,
                            QuantityInStock = s.Service_QuantityInStock ?? 0,
                            Unit = s.Service_Unit ?? string.Empty,
                            QuantityPrinciple = s.Service_QuantityPrinciple ?? 0,
                            CategoryId = s.Service_CategoryId ?? Guid.Empty,
                            OriginalQuantityId = s.Service_OriginalQuantityId ?? Guid.Empty
                        }).ToList()
            })
            .ToList();
        int totalCount = categories.Count;

        var categoryPagedResult =
            PagedResult<Response.GetServicesByCategoryIdResponse>.Create(categories, pageIndex, pageSize, totalCount);

        return Result.Success(categoryPagedResult);
    }
}
