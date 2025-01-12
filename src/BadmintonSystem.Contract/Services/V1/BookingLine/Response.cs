namespace BadmintonSystem.Contract.Services.V1.BookingLine;

public static class Response
{
    public record BookingLineResponse(
        Guid Id,
        decimal TotalPrice,
        Guid YardPriceId,
        Guid BookingId);

    public class PriceDetailResponse
    {
        public Guid? YardPriceId { get; set; }

        public decimal TotalPrice { get; set; }
    }

    public class YardPriceDetailSql
    {
        // Yard Price
        public Guid? YardPrice_Id { get; set; }

        public Guid? YardPrice_YardId { get; set; }

        public Guid? YardPrice_PriceId { get; set; }

        public Guid? YardPrice_TimeSlotId { get; set; }

        public DateTime? YardPrice_EffectiveDate { get; set; }

        public int? YardPrice_IsBooking { get; set; }

        // Price
        public Guid? Price_Id { get; set; }

        public decimal? Price_YardPrice { get; set; }

        public string? Price_Detail { get; set; }

        public string? Price_DayOfWeek { get; set; }

        public TimeSpan? Price_StartTime { get; set; }

        public TimeSpan? Price_EndTime { get; set; }

        public Guid? Price_YardTypeId { get; set; }

        public int? Price_IsDefault { get; set; }
    }
}
