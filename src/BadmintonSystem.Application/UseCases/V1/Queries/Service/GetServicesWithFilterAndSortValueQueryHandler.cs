using System.Text;
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

public sealed class GetServicesWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Service, Guid> serviceRepository)
    : IQueryHandler<Query.GetServicesWithFilterAndSortValueQuery, PagedResult<Response.ServiceDetailResponse>>
{
    public async Task<Result<PagedResult<Response.ServiceDetailResponse>>> Handle
        (Query.GetServicesWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        // Page Index and Page Size
        int PageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Service>.DefaultPageIndex
            : request.Data.PageIndex;
        int PageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Service>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Service>.UpperPageSize
                ? PagedResult<Domain.Entities.Service>.UpperPageSize
                : request.Data.PageSize;

        // Handle Query SQL
        var servicesQuery = new StringBuilder();

        servicesQuery.Append($@"SELECT * FROM ""{nameof(Domain.Entities.Service)}""
                             WHERE ""{nameof(Domain.Entities.Service.Name)}"" ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = ServiceExtension.GetSortServiceProperty(item.Key);
                servicesQuery.Append($@"AND ""{nameof(Domain.Entities.Service)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    servicesQuery.Append($@"'%{value}%', ");
                }

                servicesQuery.Length -= 2;

                servicesQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            servicesQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                servicesQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.Service)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Service)}"".""{key}"" ASC, ");
            }

            servicesQuery.Length -= 2;
        }

        servicesQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");


        List<Domain.Entities.Service> services =
            await context.Service.FromSqlRaw(servicesQuery.ToString()).ToListAsync(cancellationToken);

        int totalCount = services.Count();

        var servicePagedResult = PagedResult<Domain.Entities.Service>.Create(services, PageIndex, PageSize, totalCount);

        PagedResult<Response.ServiceDetailResponse>? result =
            mapper.Map<PagedResult<Response.ServiceDetailResponse>>(servicePagedResult);

        return Result.Success(result);
    }
}
