using System.Linq.Expressions;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Sale;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Sale;

public sealed class
    GetSalesQueryHandler(
        ApplicationDbContext context,
        IMapper mapper,
        IRepositoryBase<Domain.Entities.Sale, Guid> saleRepository)
    : IQueryHandler<Query.GetSalesQuery, PagedResult<Response.SaleResponse>>
{
    public async Task<Result<PagedResult<Response.SaleResponse>>> Handle
    (Query.GetSalesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Data.SortColumnAndOrder.Any())
        {
            int PageIndex = request.Data.PageIndex <= 0
                ? PagedResult<Domain.Entities.Sale>.DefaultPageIndex
                : request.Data.PageIndex;
            int PageSize = request.Data.PageSize <= 0
                ? PagedResult<Domain.Entities.Sale>.DefaultPageSize
                : request.Data.PageSize > PagedResult<Domain.Entities.Sale>.UpperPageSize
                    ? PagedResult<Domain.Entities.Sale>.UpperPageSize
                    : request.Data.PageSize;

            string salesQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? $@"SELECT * FROM ""{nameof(Domain.Entities.Sale)}"" ORDER BY "
                : $@"SELECT * FROM ""{nameof(Domain.Entities.Sale)}""
                              WHERE ""{nameof(Domain.Entities.Sale.Name)}"" LIKE '%{request.Data.SearchTerm}%' 
                              ORDER BY ";

            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = SaleExtension.GetSortSaleProperty(item.Key);

                salesQuery += item.Value == SortOrder.Descending
                    ? $"\"{key}\" DESC, "
                    : $"\"{key}\" ASC, ";
            }

            salesQuery = salesQuery.Remove(salesQuery.Length - 2);

            salesQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

            List<Domain.Entities.Sale> sales =
                await context.Sale.FromSqlRaw(salesQuery).ToListAsync(cancellationToken);

            int totalCount = await context.Sale.CountAsync(cancellationToken);

            var salePagedResult =
                PagedResult<Domain.Entities.Sale>.Create(sales, PageIndex, PageSize, totalCount);

            PagedResult<Response.SaleResponse>? result =
                mapper.Map<PagedResult<Response.SaleResponse>>(salePagedResult);

            return Result.Success(result);
        }
        else
        {
            IQueryable<Domain.Entities.Sale> salesQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? saleRepository.FindAll()
                : saleRepository.FindAll(x => x.Name.Contains(request.Data.SearchTerm));

            salesQuery = request.Data.SortOrder == SortOrder.Descending
                ? salesQuery.OrderByDescending(GetSortColumnProperty(request))
                : salesQuery.OrderBy(GetSortColumnProperty(request));

            var sales = await PagedResult<Domain.Entities.Sale>.CreateAsync(salesQuery, request.Data.PageIndex,
                request.Data.PageSize);

            PagedResult<Response.SaleResponse>? result = mapper.Map<PagedResult<Response.SaleResponse>>(sales);

            return Result.Success(result);
        }
    }

    private static Expression<Func<Domain.Entities.Sale, object>> GetSortColumnProperty
    (
        Query.GetSalesQuery request)
    {
        return request.Data.SortColumn?.Trim().ToLower() switch
        {
            "name" => sale => sale.Name,
            _ => sale => sale.Id
        };
    }
}
