using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Notification;

public static class Response
{
    public record NotificationResponse(
        Guid Id,
        string Content,
        int IsRead,
        Guid UserId);

    public class NotificationDetailResponse : EntityAuditBase<Guid>
    {
        public string? Content { get; set; }

        public int? IsRead { get; set; }

        public Guid? UserId { get; set; }
    }
}
