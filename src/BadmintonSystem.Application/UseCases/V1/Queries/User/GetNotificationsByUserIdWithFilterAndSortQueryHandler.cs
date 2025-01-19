using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.User;

public sealed class GetNotificationsByUserIdWithFilterAndSortQueryHandler(
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Notification, Guid> notificationRepository)
    : IQueryHandler<Query.GetNotificationsByUserIdWithFilterAndSortQuery,
        PagedResult<Response.NotificationByUserResponse>>
{
    public async Task<Result<PagedResult<Response.NotificationByUserResponse>>> Handle
        (Query.GetNotificationsByUserIdWithFilterAndSortQuery request, CancellationToken cancellationToken)
    {
        _ = await context.AppUsers.FirstOrDefaultAsync(x => x.Id == request.UserId)
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
            $@"FROM ""{nameof(Domain.Entities.Notification)}"" AS notification 
               WHERE notification.""{nameof(Domain.Entities.Notification.IsDeleted)}"" = false
               AND notification.""{nameof(Domain.Entities.Notification.UserId)}"" = '{request.UserId}' ");

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
                baseQueryBuilder.Append($@"AND notification.""{key}""::TEXT ILIKE ANY (ARRAY[");

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
                    ? $@" notification.""{key}"" DESC, "
                    : $@" notification.""{key}"" ASC, ");
            }

            // Remove trailing comma and space
            baseQueryBuilder.Length -= 2;
        }

        // Handle notification query
        var notificationQueryBuilder = new StringBuilder();

        notificationQueryBuilder.Append(
            @"SELECT notification.*");
        notificationQueryBuilder.Append(" \n");
        notificationQueryBuilder.Append(baseQueryBuilder);

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

        SqlResponse.TotalCountSqlResponse totalCountQueryResult = await notificationRepository
            .ExecuteSqlQuery<SqlResponse.TotalCountSqlResponse>(
                FormattableStringFactory.Create(countQueryBuilder.ToString()))
            .SingleAsync(cancellationToken);

        notificationQueryBuilder.Append($"\nOFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

        List<Response.NotificationByUserResponse> notification = await notificationRepository
            .ExecuteSqlQuery<Response.NotificationByUserResponse>(
                FormattableStringFactory.Create(notificationQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        var notificationPagedResult =
            PagedResult<Response.NotificationByUserResponse>.Create(
                notification,
                pageIndex,
                pageSize,
                totalCountQueryResult.TotalCount);

        return Result.Success(notificationPagedResult);
    }
}
