using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.User;

public sealed class GetAddressesByEmailWithFilterAndSortQueryHandler(
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Address, Guid> addressRepository,
    IMapper mapper)
    : IQueryHandler<Query.GetAddressesByEmailWithFilterAndSortQuery, PagedResult<Response.AddressByUserDetailResponse>>
{
    public async Task<Result<PagedResult<Response.AddressByUserDetailResponse>>> Handle
        (Query.GetAddressesByEmailWithFilterAndSortQuery request, CancellationToken cancellationToken)
    {
        _ = context.AppUsers.FirstOrDefault(x => x.Id == request.UserId)
            ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        // Pagination
        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Address>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Address>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Address>.UpperPageSize
                ? PagedResult<Domain.Entities.Address>.UpperPageSize
                : request.Data.PageSize;

        var baseQueryBuilder = new StringBuilder();
        baseQueryBuilder.Append(
            $@"FROM ""{nameof(Domain.Entities.Address)}"" AS address 
                JOIN ""{nameof(UserAddress)}"" AS userAddress
                ON userAddress.""{nameof(UserAddress.AddressId)}"" = address.""{nameof(Domain.Entities.Address.Id)}""
                WHERE address.""{nameof(Domain.Entities.Address.IsDeleted)}"" = false
                AND userAddress.""{nameof(UserAddress.UserId)}"" = '{request.UserId}' ");

        // SEARCH
        if (!string.IsNullOrWhiteSpace(request.Data.SearchTerm))
        {
            baseQueryBuilder.Append(
                $@"AND ""{nameof(Domain.Entities.Address.Province)}"" ILIKE '%{request.Data.SearchTerm}%' ");
        }

        // FILTER MULTIPLE
        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = AddressExtension.GetSortAddressProperty(item.Key);
                baseQueryBuilder.Append($@"AND address.""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    baseQueryBuilder.Append($@"'%{value}%', ");
                }

                // Remove trailing comma
                baseQueryBuilder.Length -= 2;

                baseQueryBuilder.Append("]) ");
            }
        }

        // SORT MULTIPLE
        if (request.Data.SortColumnAndOrder.Any())
        {
            baseQueryBuilder.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = AddressExtension.GetSortAddressProperty(item.Key);
                baseQueryBuilder.Append(item.Value == SortOrder.Descending
                    ? $@" address.""{key}"" DESC, "
                    : $@" address.""{key}"" ASC, ");
            }

            // Remove trailing comma and space
            baseQueryBuilder.Length -= 2;
        }

        // Handle address query
        var addressesQueryBuilder = new StringBuilder();

        addressesQueryBuilder.Append(
            $@"SELECT address.*, userAddress.""{nameof(UserAddress.IsDefault)}""");
        addressesQueryBuilder.Append(" \n");
        addressesQueryBuilder.Append(baseQueryBuilder);

        // Calculate total count record
        var countQueryBuilder = new StringBuilder();
        countQueryBuilder.Append(
            $@"SELECT COUNT(*) AS ""{nameof(SqlResponse.TotalCountSqlResponse.TotalCount)}""");
        countQueryBuilder.Append(" \n");

        // Exclude ORDER BY from baseQueryBuilder
        string baseQueryWithoutOrderBy = baseQueryBuilder.ToString();
        int orderByIndex = baseQueryWithoutOrderBy.LastIndexOf("ORDER BY", StringComparison.OrdinalIgnoreCase);
        if (orderByIndex > -1)
        {
            baseQueryWithoutOrderBy = baseQueryWithoutOrderBy.Substring(0, orderByIndex);
        }

        countQueryBuilder.Append(baseQueryWithoutOrderBy);

        SqlResponse.TotalCountSqlResponse totalCountQueryResult = await addressRepository
            .ExecuteSqlQuery<SqlResponse.TotalCountSqlResponse>(
                FormattableStringFactory.Create(countQueryBuilder.ToString()))
            .SingleAsync(cancellationToken);

        addressesQueryBuilder.Append($"\nOFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

        List<Response.AddressByUserDetailResponse> addresses = await addressRepository
            .ExecuteSqlQuery<Response.AddressByUserDetailResponse>(
                FormattableStringFactory.Create(addressesQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        var addressPagedResult =
            PagedResult<Response.AddressByUserDetailResponse>.Create(
                addresses,
                pageIndex,
                pageSize,
                totalCountQueryResult.TotalCount);

        return Result.Success(addressPagedResult);
    }
}
