using System.Linq.Expressions;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Yard;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Yard;

public sealed class
    GetYardsQueryHandler(
        ApplicationDbContext context,
        IMapper mapper,
        IRepositoryBase<Domain.Entities.Yard, Guid> yardRepository)
    : IQueryHandler<Query.GetYardsQuery, PagedResult<Response.YardResponse>>
{
    public async Task<Result<PagedResult<Response.YardResponse>>> Handle
    (Query.GetYardsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Data.SortColumnAndOrder.Any())
        {
            int PageIndex = request.Data.PageIndex <= 0
                ? PagedResult<Domain.Entities.Yard>.DefaultPageIndex
                : request.Data.PageIndex;
            int PageSize = request.Data.PageSize <= 0
                ? PagedResult<Domain.Entities.Yard>.DefaultPageSize
                : request.Data.PageSize > PagedResult<Domain.Entities.Yard>.UpperPageSize
                    ? PagedResult<Domain.Entities.Yard>.UpperPageSize
                    : request.Data.PageSize;

            string yardsQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? $@"SELECT * FROM ""{nameof(Domain.Entities.Yard)}"" ORDER BY "
                : $@"SELECT * FROM ""{nameof(Domain.Entities.Yard)}""
                              WHERE ""{nameof(Domain.Entities.Yard.Name)}"" LIKE '%{request.Data.SearchTerm}%' 
                              ORDER BY ";

            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = YardExtension.GetSortYardProperty(item.Key);

                yardsQuery += item.Value == SortOrder.Descending
                    ? $"\"{key}\" DESC, "
                    : $"\"{key}\" ASC, ";
            }

            yardsQuery = yardsQuery.Remove(yardsQuery.Length - 2);

            yardsQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

            List<Domain.Entities.Yard> yards =
                await context.Yard.FromSqlRaw(yardsQuery).ToListAsync(cancellationToken);

            int totalCount = await context.Yard.CountAsync(cancellationToken);

            var yardPagedResult =
                PagedResult<Domain.Entities.Yard>.Create(yards, PageIndex, PageSize, totalCount);

            PagedResult<Response.YardResponse>? result =
                mapper.Map<PagedResult<Response.YardResponse>>(yardPagedResult);

            return Result.Success(result);
        }
        else
        {
            IQueryable<Domain.Entities.Yard> yardsQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? yardRepository.FindAll()
                : yardRepository.FindAll(x => x.Name.Contains(request.Data.SearchTerm));

            yardsQuery = request.Data.SortOrder == SortOrder.Descending
                ? yardsQuery.OrderByDescending(GetSortColumnProperty(request))
                : yardsQuery.OrderBy(GetSortColumnProperty(request));

            var yards = await PagedResult<Domain.Entities.Yard>.CreateAsync(yardsQuery, request.Data.PageIndex,
                request.Data.PageSize);

            PagedResult<Response.YardResponse>? result = mapper.Map<PagedResult<Response.YardResponse>>(yards);

            return Result.Success(result);
        }
    }

    private static Expression<Func<Domain.Entities.Yard, object>> GetSortColumnProperty
    (
        Query.GetYardsQuery request)
    {
        return request.Data.SortColumn?.Trim().ToLower() switch
        {
            "name" => yard => yard.Name,
            _ => yard => yard.Id
        };
    }
}
