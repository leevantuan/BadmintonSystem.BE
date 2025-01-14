using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Provider;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Provider;

public sealed class GetProvidersWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Provider, Guid> providerRepository)
    : IQueryHandler<Query.GetProvidersWithFilterAndSortValueQuery, PagedResult<Response.ProviderDetailResponse>>
{
    public async Task<Result<PagedResult<Response.ProviderDetailResponse>>> Handle
        (Query.GetProvidersWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        // Page Index and Page Size
        int PageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Provider>.DefaultPageIndex
            : request.Data.PageIndex;
        int PageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Provider>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Provider>.UpperPageSize
                ? PagedResult<Domain.Entities.Provider>.UpperPageSize
                : request.Data.PageSize;

        // Handle Query SQL
        var providersQuery = new StringBuilder();

        providersQuery.Append($@"SELECT * FROM ""{nameof(Domain.Entities.Provider)}""
                             WHERE ""{nameof(Domain.Entities.Provider.Name)}"" ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = ProviderExtension.GetSortProviderProperty(item.Key);
                providersQuery.Append(
                    $@"AND ""{nameof(Domain.Entities.Provider)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    providersQuery.Append($@"'%{value}%', ");
                }

                providersQuery.Length -= 2;

                providersQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            providersQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                providersQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.Provider)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Provider)}"".""{key}"" ASC, ");
            }

            providersQuery.Length -= 2;
        }

        providersQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");


        List<Domain.Entities.Provider> providers =
            await context.Provider.FromSqlRaw(providersQuery.ToString()).ToListAsync(cancellationToken);

        int totalCount = providers.Count();

        var providerPagedResult =
            PagedResult<Domain.Entities.Provider>.Create(providers, PageIndex, PageSize, totalCount);

        PagedResult<Response.ProviderDetailResponse>? result =
            mapper.Map<PagedResult<Response.ProviderDetailResponse>>(providerPagedResult);

        return Result.Success(result);
    }
}
