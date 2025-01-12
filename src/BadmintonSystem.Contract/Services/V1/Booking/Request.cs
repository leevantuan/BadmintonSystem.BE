namespace BadmintonSystem.Contract.Services.V1.Booking;

public static class Request
{
    public class CreateBooking
    {
        public DateTime? BookingDate { get; set; }

        public decimal? BookingTotal { get; set; }

        public Guid? UserId { get; set; }

        public Guid? SaleId { get; set; }

        public int? BookingStatus { get; set; }

        public int? PaymentStatus { get; set; }
    }

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

    public class CreateBookingRequest
    {
        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? SaleId { get; set; }

        public int PercentPrePay { get; set; }

        public List<Guid> YardPriceIds { get; set; }
    }
}
