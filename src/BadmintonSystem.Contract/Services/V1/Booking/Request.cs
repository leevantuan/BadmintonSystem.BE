namespace BadmintonSystem.Contract.Services.V1.Booking;

public static class Request
{
    public record CreateBookingRequest(
        DateTime BookingDate,
        decimal BookingTotal,
        int PaymentStatus,
        int BookingStatus,
        Guid SaleId,
        Guid UserId);

    public class UpdateBookingRequest
    {
        public Guid Id { get; set; }

        public DateTime? BookingDate { get; set; }

        public decimal? BookingTotal { get; set; }

        public Guid? UserId { get; set; }

        public Guid? SaleId { get; set; }

        public int? BookingStatus { get; set; }

        public int? PaymentStatus { get; set; }
    }
}
