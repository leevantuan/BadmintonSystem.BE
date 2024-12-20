using AutoMapper;
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
    : IQueryHandler<Query.GetNotificationsByIdQuery, Response.NotificationDetailResponse>
{
    public async Task<Result<Response.NotificationDetailResponse>> Handle
        (Query.GetNotificationsByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Notification notification =
            await notificationRepository.FindByIdAsync(request.NotificationId, cancellationToken)
            ?? throw new NotificationException.NotificationNotFoundException(request.NotificationId);

        Response.NotificationDetailResponse? result = mapper.Map<Response.NotificationDetailResponse>(notification);

        return Result.Success(result);
    }
}
