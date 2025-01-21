using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Booking;

public static class DomainEvent
{
    public record BookingDone(Guid Id)
        : IDomainEvent;

    public record BookingCancelled(Guid Id)
        : IDomainEvent;
}
