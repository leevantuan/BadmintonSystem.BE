namespace BadmintonSystem.Contract.Services.V1.Booking;

public static class Request
{
    public class CreateBooking
    {
        public Guid Id { get; set; }

        public DateTime BookingDate { get; set; }

        public decimal BookingTotal { get; set; }

        public decimal OriginalPrice { get; set; }

        public int BookingStatus { get; set; }

        public int PaymentStatus { get; set; }

        public Guid? UserId { get; set; }

        public Guid? SaleId { get; set; }

        public int? PercentPrePay { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }
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

    // Booking By Chat
    public class CreateBookingByChatRequest
    {
        public string Email { get; set; }

        public string Tenant { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime BookingDate { get; set; }
    }

    // Booking By Chat
    public class CreateUrlBookingByChatRequest
    {
        public string Email { get; set; }

        public string Tenant { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string BookingDate { get; set; }
    }

    public class CheckUnBookedByChatRequest
    {
        public string Tenant { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime BookingDate { get; set; }
    }

    public class CreateBookingRequest
    {
        public string? FullName { get; set; }

        public string? Email { get; set; }

        public Guid? UserId { get; set; }

        public string? Tenant { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? SaleId { get; set; }

        public int PercentPrePay { get; set; }

        public List<Guid> YardPriceIds { get; set; }
    }

    public class ReserveBookingRequest
    {
        public string IsToken { get; set; }

        public string Type { get; set; }
    }
}
