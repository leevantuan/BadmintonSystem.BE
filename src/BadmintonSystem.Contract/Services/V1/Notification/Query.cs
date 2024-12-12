using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Notification;

public static class Query
{
    public record GetNotificationsQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.NotificationResponse>>;

    public record GetNotificationByIdQuery(Guid Id)
        : IQuery<Response.NotificationResponse>;

    public record GetNotificationsWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.NotificationDetailResponse>>;
}
