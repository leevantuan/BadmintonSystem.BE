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

    public class YardPriceDetailResponse : EntityBase<Guid>
    {
        public Guid YardId { get; set; }

        public Guid? PriceId { get; set; }

        public Guid TimeSlotId { get; set; }

        public DateTime EffectiveDate { get; set; }

        public int IsBooking { get; set; }
    }
}
