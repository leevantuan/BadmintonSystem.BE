namespace BadmintonSystem.Contract.Services.V1.BookingLine;

public static class Request
{
    public record CreateBookingLineRequest(
        decimal TotalPrice,
        Guid YardId,
        Guid BookingId);

    public class UpdateBookingLineRequest
    {
        public Guid Id { get; set; }

        public decimal? TotalPrice { get; set; }

        public Guid? YardId { get; set; }

        public Guid? BookingId { get; set; }
    }
}
