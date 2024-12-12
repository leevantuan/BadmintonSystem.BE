using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Notification;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Notification;

public sealed class GetNotificationsWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Notification, Guid> notificationRepository)
    : IQueryHandler<Query.GetNotificationsWithFilterAndSortValueQuery, PagedResult<Response.NotificationDetailResponse>>
{
    public async Task<Result<PagedResult<Response.NotificationDetailResponse>>> Handle
        (Query.GetNotificationsWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        // Page Index and Page Size
        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Notification>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Notification>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Notification>.UpperPageSize
                ? PagedResult<Domain.Entities.Notification>.UpperPageSize
                : request.Data.PageSize;

        // Handle Query SQL
        var notificationsQuery = new StringBuilder();

        notificationsQuery.Append($@"SELECT * FROM ""{nameof(Domain.Entities.Notification)}""
                             WHERE ""{nameof(Domain.Entities.Notification.Content)}"" ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = NotificationExtension.GetSortNotificationProperty(item.Key);
                notificationsQuery.Append(
                    $@"AND ""{nameof(Domain.Entities.Notification)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    notificationsQuery.Append($@"'%{value}%', ");
                }

                notificationsQuery.Length -= 2;

                notificationsQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            notificationsQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                notificationsQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.Notification)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Notification)}"".""{key}"" ASC, ");
            }

            notificationsQuery.Length -= 2;
        }

        notificationsQuery.Append($"\nOFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

        List<Domain.Entities.Notification> notifications =
            await context.Notification.FromSqlRaw(notificationsQuery.ToString()).ToListAsync(cancellationToken);

        int totalCount = notifications.Count();

        var notificationPagedResult =
            PagedResult<Domain.Entities.Notification>.Create(notifications, pageIndex, pageSize, totalCount);

        PagedResult<Response.NotificationDetailResponse>? result =
            mapper.Map<PagedResult<Response.NotificationDetailResponse>>(notificationPagedResult);

        return Result.Success(result);
    }
}
