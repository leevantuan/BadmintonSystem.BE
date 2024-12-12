namespace BadmintonSystem.Contract.Services.V1.Notification;

public static class Request
{
    public record CreateNotificationRequest(
        string Content,
        Guid UserId);

    public class UpdateNotificationRequest
    {
        public Guid Id { get; set; }

        public string? Content { get; set; }

        public Guid UserId { get; set; }
    }
}
