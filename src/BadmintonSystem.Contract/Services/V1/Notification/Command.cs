using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Notification;

public static class Command
{
    public record CreateNotificationCommand(Guid UserId, Request.CreateNotificationRequest Data)
        : ICommand<Response.NotificationResponse>;

    public record UpdateNotificationCommand(Request.UpdateNotificationRequest Data)
        : ICommand<Response.NotificationResponse>;

    public record DeleteNotificationsCommand(List<string> Ids)
        : ICommand;
}
