using System.Runtime.CompilerServices;
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
        int PageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Service>.DefaultPageIndex
            : request.Data.PageIndex;
        int PageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Service>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Service>.UpperPageSize
                ? PagedResult<Domain.Entities.Service>.UpperPageSize
                : request.Data.PageSize;

        var baseQuery = new StringBuilder();
        baseQuery.Append($@"FROM ""{nameof(Domain.Entities.Service)}""
                             WHERE ""{nameof(Domain.Entities.Service.Name)}"" ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = ServiceExtension.GetSortServiceProperty(item.Key);
                baseQuery.Append($@"AND ""{nameof(Domain.Entities.Service)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

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
                    ? $@" ""{nameof(Domain.Entities.Service)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Service)}"".""{key}"" ASC, ");
            }

            baseQuery.Length -= 2;
        }

        int totalCount = await TotalCount(baseQuery.ToString(), cancellationToken);

        var servicesQuery = new StringBuilder();
        servicesQuery.Append(@"SELECT * ");
        servicesQuery.Append(baseQuery);
        servicesQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");


        List<Domain.Entities.Service> services =
            await context.Service.FromSqlRaw(servicesQuery.ToString()).ToListAsync(cancellationToken);

        var servicePagedResult = PagedResult<Domain.Entities.Service>.Create(services, PageIndex, PageSize, totalCount);

        PagedResult<Response.ServiceDetailResponse>? result =
            mapper.Map<PagedResult<Response.ServiceDetailResponse>>(servicePagedResult);

        return Result.Success(result);
    }

    private async Task<int> TotalCount(string baseQuery, CancellationToken cancellationToken)
    {
        var countQueryBuilder = new StringBuilder();
        countQueryBuilder.Append(
            $@"SELECT COUNT(*) AS ""{nameof(SqlResponse.TotalCountSqlResponse.TotalCount)}""");
        countQueryBuilder.Append(" \n");

        countQueryBuilder.Append(baseQuery);
        SqlResponse.TotalCountSqlResponse totalCountQueryResult = await serviceRepository
            .ExecuteSqlQuery<SqlResponse.TotalCountSqlResponse>(
                FormattableStringFactory.Create(countQueryBuilder.ToString()))
            .SingleAsync(cancellationToken);

        return totalCountQueryResult.TotalCount;
    }
}
