using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Notification;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Notification;

public sealed class UpdateNotificationCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Notification, Guid> notificationRepository)
    : ICommandHandler<Command.UpdateNotificationCommand, Response.NotificationResponse>
{
    public async Task<Result<Response.NotificationResponse>> Handle
        (Command.UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Notification notification =
            await notificationRepository.FindByIdAsync(request.Data.Id, cancellationToken)
            ?? throw new NotificationException.NotificationNotFoundException(request.Data.Id);

        notification.Content = request.Data.Content ?? notification.Content;
        notification.UserId = request.Data.UserId;

        Response.NotificationResponse? result = mapper.Map<Response.NotificationResponse>(notification);

        return Result.Success(result);
    }
}
