using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Notification;
using BadmintonSystem.Domain.Abstractions.Repositories;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Notification;

public sealed class CreateNotificationCommandHandler(
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Notification, Guid> notificationRepository)
    : ICommandHandler<Command.CreateNotificationCommand, Response.NotificationResponse>
{
    public Task<Result<Response.NotificationResponse>> Handle
        (Command.CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Notification notification = mapper.Map<Domain.Entities.Notification>(request.Data);

        notificationRepository.Add(notification);

        Response.NotificationResponse? result = mapper.Map<Response.NotificationResponse>(notification);

        return Task.FromResult(Result.Success(result));
    }
}
