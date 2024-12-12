namespace BadmintonSystem.Domain.Exceptions;
public static class NotificationException
{
    public class NotificationNotFoundException : NotFoundException
    {
        public NotificationNotFoundException(Guid notificationId)
            : base($"The notification with the id {notificationId} was not found.")
        { }
    }
}
