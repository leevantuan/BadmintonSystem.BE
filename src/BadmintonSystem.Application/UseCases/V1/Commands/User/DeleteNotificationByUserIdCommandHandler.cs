using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.User;

public sealed class DeleteNotificationByUserIdCommandHandler(
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Notification, Guid> notificationRepository,
    IMapper mapper)
    : ICommandHandler<Command.DeleteNotificationByUserIdCommand>
{
    public async Task<Result> Handle
        (Command.DeleteNotificationByUserIdCommand request, CancellationToken cancellationToken)
    {
        _ = await context.AppUsers.FirstOrDefaultAsync(x => x.Id == request.UserId)
            ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        Domain.Entities.Notification notification =
            await notificationRepository.FindByIdAsync(request.NotificationId, cancellationToken)
            ?? throw new NotificationException.NotificationNotFoundException(request.NotificationId);

        notificationRepository.Remove(notification);

        return Result.Success();
    }
}
