namespace BadmintonSystem.Contract.Services.V1.YardPrice;

public static class Request
{
    public record CreateYardPriceRequest(
        Guid YardId,
        Guid? PriceId,
        Guid TimeSlotId,
        DateTime EffectiveDate,
        int IsBooking);

    public class UpdateYardPriceRequest
    {
        public Guid Id { get; set; }

        public Guid YardId { get; set; }

        public Guid? PriceId { get; set; }

        public Guid TimeSlotId { get; set; }

        public DateTime EffectiveDate { get; set; }

        public int IsBooking { get; set; }
    }

    public class GetYardPricesFreeByDateRequest
    {
        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Tenant { get; set; }
    }
}
