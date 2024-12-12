﻿using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Notification;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Notification;

public sealed class GetNotificationByIdQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Notification, Guid> notificationRepository)
    : IQueryHandler<Query.GetNotificationByIdQuery, Response.NotificationResponse>
{
    public async Task<Result<Response.NotificationResponse>> Handle
        (Query.GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Notification notification =
            await notificationRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new NotificationException.NotificationNotFoundException(request.Id);

        Response.NotificationResponse? result = mapper.Map<Response.NotificationResponse>(notification);

        return Result.Success(result);
    }
}
