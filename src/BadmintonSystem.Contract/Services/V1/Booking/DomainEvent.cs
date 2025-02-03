using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Booking;

public static class DomainEvent
{
    public record BookingDone(List<Guid> Ids, string Name, string Email)
        : IDomainEvent;

    public record BookingNotificationToStaff(List<Guid> Ids, string Name, string Email)
        : IDomainEvent;

    public record BookingCancelled(List<Guid> Ids)
        : IDomainEvent;
}
