using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Notification;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Notification;

public sealed class DeleteNotificationsCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Notification, Guid> notificationRepository)
    : ICommandHandler<Command.DeleteNotificationsCommand>
{
    public async Task<Result> Handle(Command.DeleteNotificationsCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Notification> notifications = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.Notification notification =
                await notificationRepository.FindByIdAsync(idValue, cancellationToken)
                ?? throw new NotificationException.NotificationNotFoundException(idValue);

            notifications.Add(notification);
        }

        notificationRepository.RemoveMultiple(notifications);

        return Result.Success();
    }
}
