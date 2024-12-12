using System.Linq.Expressions;
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

public sealed class
    GetNotificationsQueryHandler(
        ApplicationDbContext context,
        IMapper mapper,
        IRepositoryBase<Domain.Entities.Notification, Guid> notificationRepository)
    : IQueryHandler<Query.GetNotificationsQuery, PagedResult<Response.NotificationResponse>>
{
    public async Task<Result<PagedResult<Response.NotificationResponse>>> Handle
    (Query.GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Data.SortColumnAndOrder.Any())
        {
            int PageIndex = request.Data.PageIndex <= 0
                ? PagedResult<Domain.Entities.Notification>.DefaultPageIndex
                : request.Data.PageIndex;
            int PageSize = request.Data.PageSize <= 0
                ? PagedResult<Domain.Entities.Notification>.DefaultPageSize
                : request.Data.PageSize > PagedResult<Domain.Entities.Notification>.UpperPageSize
                    ? PagedResult<Domain.Entities.Notification>.UpperPageSize
                    : request.Data.PageSize;

            string notificationsQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? $@"SELECT * FROM ""{nameof(Domain.Entities.Notification)}"" ORDER BY "
                : $@"SELECT * FROM ""{nameof(Domain.Entities.Notification)}""
                              WHERE ""{nameof(Domain.Entities.Notification.Content)}"" LIKE '%{request.Data.SearchTerm}%' 
                              ORDER BY ";

            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = NotificationExtension.GetSortNotificationProperty(item.Key);

                notificationsQuery += item.Value == SortOrder.Descending
                    ? $"\"{key}\" DESC, "
                    : $"\"{key}\" ASC, ";
            }

            notificationsQuery = notificationsQuery.Remove(notificationsQuery.Length - 2);

            notificationsQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

            List<Domain.Entities.Notification> notifications =
                await context.Notification.FromSqlRaw(notificationsQuery).ToListAsync(cancellationToken);

            int totalCount = await context.Notification.CountAsync(cancellationToken);

            var notificationPagedResult =
                PagedResult<Domain.Entities.Notification>.Create(notifications, PageIndex, PageSize, totalCount);

            PagedResult<Response.NotificationResponse>? result =
                mapper.Map<PagedResult<Response.NotificationResponse>>(notificationPagedResult);

            return Result.Success(result);
        }
        else
        {
            IQueryable<Domain.Entities.Notification> notificationsQuery =
                string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                    ? notificationRepository.FindAll()
                    : notificationRepository.FindAll(x => x.Content.Contains(request.Data.SearchTerm));

            notificationsQuery = request.Data.SortOrder == SortOrder.Descending
                ? notificationsQuery.OrderByDescending(GetSortColumnProperty(request))
                : notificationsQuery.OrderBy(GetSortColumnProperty(request));

            var notifications = await PagedResult<Domain.Entities.Notification>.CreateAsync(notificationsQuery,
                request.Data.PageIndex,
                request.Data.PageSize);

            PagedResult<Response.NotificationResponse>? result =
                mapper.Map<PagedResult<Response.NotificationResponse>>(notifications);

            return Result.Success(result);
        }
    }

    private static Expression<Func<Domain.Entities.Notification, object>> GetSortColumnProperty
    (
        Query.GetNotificationsQuery request)
    {
        return request.Data.SortColumn?.Trim().ToLower() switch
        {
            "name" => notification => notification.Content,
            _ => notification => notification.Id
        };
    }
}
