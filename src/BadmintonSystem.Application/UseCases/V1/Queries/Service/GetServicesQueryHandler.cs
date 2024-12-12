using System.Linq.Expressions;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Service;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Service;

public sealed class
    GetServicesQueryHandler(
        ApplicationDbContext context,
        IMapper mapper,
        IRepositoryBase<Domain.Entities.Service, Guid> serviceRepository)
    : IQueryHandler<Query.GetServicesQuery, PagedResult<Response.ServiceResponse>>
{
    public async Task<Result<PagedResult<Response.ServiceResponse>>> Handle
    (Query.GetServicesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Data.SortColumnAndOrder.Any())
        {
            int PageIndex = request.Data.PageIndex <= 0
                ? PagedResult<Domain.Entities.Service>.DefaultPageIndex
                : request.Data.PageIndex;
            int PageSize = request.Data.PageSize <= 0
                ? PagedResult<Domain.Entities.Service>.DefaultPageSize
                : request.Data.PageSize > PagedResult<Domain.Entities.Service>.UpperPageSize
                    ? PagedResult<Domain.Entities.Service>.UpperPageSize
                    : request.Data.PageSize;

            string servicesQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? $@"SELECT * FROM ""{nameof(Domain.Entities.Service)}"" ORDER BY "
                : $@"SELECT * FROM ""{nameof(Domain.Entities.Service)}""
                              WHERE ""{nameof(Domain.Entities.Service.Name)}"" LIKE '%{request.Data.SearchTerm}%' 
                              ORDER BY ";

            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ServiceExtension.GetSortServiceProperty(item.Key);

                servicesQuery += item.Value == SortOrder.Descending
                    ? $"\"{key}\" DESC, "
                    : $"\"{key}\" ASC, ";
            }

            servicesQuery = servicesQuery.Remove(servicesQuery.Length - 2);

            servicesQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

            List<Domain.Entities.Service> services =
                await context.Service.FromSqlRaw(servicesQuery).ToListAsync(cancellationToken);

            int totalCount = await context.Service.CountAsync(cancellationToken);

            var servicePagedResult =
                PagedResult<Domain.Entities.Service>.Create(services, PageIndex, PageSize, totalCount);

            PagedResult<Response.ServiceResponse>? result =
                mapper.Map<PagedResult<Response.ServiceResponse>>(servicePagedResult);

            return Result.Success(result);
        }
        else
        {
            IQueryable<Domain.Entities.Service> servicesQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? serviceRepository.FindAll()
                : serviceRepository.FindAll(x => x.Name.Contains(request.Data.SearchTerm));

            servicesQuery = request.Data.SortOrder == SortOrder.Descending
                ? servicesQuery.OrderByDescending(GetSortColumnProperty(request))
                : servicesQuery.OrderBy(GetSortColumnProperty(request));

            var services = await PagedResult<Domain.Entities.Service>.CreateAsync(servicesQuery, request.Data.PageIndex,
                request.Data.PageSize);

            PagedResult<Response.ServiceResponse>? result = mapper.Map<PagedResult<Response.ServiceResponse>>(services);

            return Result.Success(result);
        }
    }

    private static Expression<Func<Domain.Entities.Service, object>> GetSortColumnProperty
    (
        Query.GetServicesQuery request)
    {
        return request.Data.SortColumn?.Trim().ToLower() switch
        {
            "name" => service => service.Name,
            _ => service => service.Id
        };
    }
}
