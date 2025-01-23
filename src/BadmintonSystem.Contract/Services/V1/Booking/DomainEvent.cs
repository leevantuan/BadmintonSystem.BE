using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Booking;

public static class DomainEvent
{
    public record BookingDone(List<Guid> Ids)
        : IDomainEvent;

    public record BookingCancelled(List<Guid> Ids)
        : IDomainEvent;
}
