namespace BadmintonSystem.Contract.Services.V1.BookingTime;

public static class Request
{
    public record CreateBookingTimeRequest(
        Guid TimeSlotId,
        Guid BookingLineId);

    public record DeleteBookingTimeRequest(
        Guid TimeSlotId,
        Guid BookingLineId);

    public class UpdateBookingTimeRequest
    {
        public Guid TimeSlotId { get; set; }

        public Guid? BookingLineId { get; set; }
    }
}
