using System.Linq.Expressions;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.YardType;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.YardType;

public sealed class
    GetYardTypesQueryHandler(
        ApplicationDbContext context,
        IMapper mapper,
        IRepositoryBase<Domain.Entities.YardType, Guid> yardTypeRepository)
    : IQueryHandler<Query.GetYardTypesQuery, PagedResult<Response.YardTypeResponse>>
{
    public async Task<Result<PagedResult<Response.YardTypeResponse>>> Handle
    (Query.GetYardTypesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Data.SortColumnAndOrder.Any())
        {
            int PageIndex = request.Data.PageIndex <= 0
                ? PagedResult<Domain.Entities.YardType>.DefaultPageIndex
                : request.Data.PageIndex;
            int PageSize = request.Data.PageSize <= 0
                ? PagedResult<Domain.Entities.YardType>.DefaultPageSize
                : request.Data.PageSize > PagedResult<Domain.Entities.YardType>.UpperPageSize
                    ? PagedResult<Domain.Entities.YardType>.UpperPageSize
                    : request.Data.PageSize;

            string yardTypesQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? $@"SELECT * FROM ""{nameof(Domain.Entities.YardType)}"" ORDER BY "
                : $@"SELECT * FROM ""{nameof(Domain.Entities.YardType)}""
                              WHERE ""{nameof(Domain.Entities.YardType.Name)}"" LIKE '%{request.Data.SearchTerm}%' 
                              ORDER BY ";

            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = YardTypeExtension.GetSortYardTypeProperty(item.Key);

                yardTypesQuery += item.Value == SortOrder.Descending
                    ? $"\"{key}\" DESC, "
                    : $"\"{key}\" ASC, ";
            }

            yardTypesQuery = yardTypesQuery.Remove(yardTypesQuery.Length - 2);

            yardTypesQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

            List<Domain.Entities.YardType> yardTypes =
                await context.YardType.FromSqlRaw(yardTypesQuery).ToListAsync(cancellationToken);

            int totalCount = await context.YardType.CountAsync(cancellationToken);

            var yardTypePagedResult =
                PagedResult<Domain.Entities.YardType>.Create(yardTypes, PageIndex, PageSize, totalCount);

            PagedResult<Response.YardTypeResponse>? result =
                mapper.Map<PagedResult<Response.YardTypeResponse>>(yardTypePagedResult);

            return Result.Success(result);
        }
        else
        {
            IQueryable<Domain.Entities.YardType> yardTypesQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? yardTypeRepository.FindAll()
                : yardTypeRepository.FindAll(x => x.Name.Contains(request.Data.SearchTerm));

            yardTypesQuery = request.Data.SortOrder == SortOrder.Descending
                ? yardTypesQuery.OrderByDescending(GetSortColumnProperty(request))
                : yardTypesQuery.OrderBy(GetSortColumnProperty(request));

            var yardTypes = await PagedResult<Domain.Entities.YardType>.CreateAsync(yardTypesQuery,
                request.Data.PageIndex,
                request.Data.PageSize);

            PagedResult<Response.YardTypeResponse>? result =
                mapper.Map<PagedResult<Response.YardTypeResponse>>(yardTypes);

            return Result.Success(result);
        }
    }

    private static Expression<Func<Domain.Entities.YardType, object>> GetSortColumnProperty
    (
        Query.GetYardTypesQuery request)
    {
        return request.Data.SortColumn?.Trim().ToLower() switch
        {
            "name" => yardType => yardType.Name,
            _ => yardType => yardType.Id
        };
    }
}
