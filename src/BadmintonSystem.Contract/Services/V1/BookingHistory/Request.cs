namespace BadmintonSystem.Contract.Services.V1.BookingHistory;

public static class Request
{
    public class CreateBookingHistoryRequest
    {
        public Guid BookingId { get; set; }

        public string? ClubName { get; set; }

        public TimeSpan? StartTime { get; set; }

        public DateTime? PlayDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public decimal? TotalPrice { get; set; }

        public int? PaymentStatus { get; set; }

        public string? TenantCode { get; set; }

        public Guid? UserId { get; set; }
    }
}
