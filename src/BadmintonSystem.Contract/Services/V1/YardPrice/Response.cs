using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.YardPrice;

public static class Response
{
    public record YardPriceResponse(
        Guid Id,
        Guid YardId,
        Guid? PriceId,
        Guid TimeSlotId,
        DateTime EffectiveDate,
        int IsBooking);

    public class YardPriceDetail : EntityBase<Guid>
    {
        public Guid YardId { get; set; }

        public Guid? PriceId { get; set; }

        public Guid TimeSlotId { get; set; }

        public DateTime EffectiveDate { get; set; }

        public int IsBooking { get; set; }
    }

    public class YardPriceDetailResponse : YardPriceDetail
    {
        public Yard.Response.YardResponse? Yard { get; set; }

        public TimeSlot.Response.TimeSlotResponse? TimeSlot { get; set; }

        public Price.Response.PriceResponse? Price { get; set; }
    }

    public class YardPricesByDateDetailResponse
    {
        public Yard.Response.YardResponse? Yard { get; set; }

        public List<YardPricesByDateDetail>? YardPricesDetails { get; set; }
    }

    public class YardPricesByDateDetail : YardPriceDetail
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public decimal Price { get; set; }

        public string? IsToken { get; set; }

        public DateTime? ExpirationTime { get; set; }
    }

    public class YardPriceDetailSql
    {
        public Guid? YardPrice_Id { get; set; }

        public Guid? YardPrice_YardId { get; set; }

        public Guid? YardPrice_PriceId { get; set; }

        public Guid? YardPrice_TimeSlotId { get; set; }

        public DateTime? YardPrice_EffectiveDate { get; set; }

        public int? YardPrice_IsBooking { get; set; }

        // Yard Id
        public Guid? Yard_Id { get; set; }

        public string? Yard_Name { get; set; }

        public Guid? Yard_YardTypeId { get; set; }

        public int? Yard_IsStatus { get; set; }

        // Time Slot Id
        public Guid? TimeSlot_Id { get; set; }

        public TimeSpan? TimeSlot_StartTime { get; set; }

        public TimeSpan? TimeSlot_EndTime { get; set; }

        // Price Id
        public Guid? Price_Id { get; set; }

        public decimal? Price_YardPrice { get; set; }

        public int? Price_IsDefault { get; set; }
    }
}
