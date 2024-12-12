namespace BadmintonSystem.Contract.Services.V1.BookingTime;

public static class Response
{
    public record BookingTimeResponse(
        Guid TimeSlotId,
        Guid BookingLineId);

    public class BookingTimeDetailResponse
    {
        public Guid TimeSlotId { get; set; }

        public Guid? BookingLineId { get; set; }
    }
}
