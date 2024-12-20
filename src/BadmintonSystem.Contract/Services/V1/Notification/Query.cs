﻿using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Notification;

public static class Query
{
    public record GetNotificationsQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.NotificationResponse>>;

    public record GetNotificationsByIdQuery(
        Guid UserId,
        Guid NotificationId)
        : IQuery<Response.NotificationDetailResponse>;

    public record GetNotificationsWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.NotificationDetailResponse>>;
}
