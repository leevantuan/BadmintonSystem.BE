using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Price;

public static class Response
{
    public class PriceResponse
    {
        public Guid Id { get; set; }

        public decimal YardPrice { get; set; }

        public string? Detail { get; set; }

        public string? DayOfWeek { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public Guid? YardTypeId { get; set; }

        public int IsDefault { get; set; }
    }

    public class PriceDetailResponse : EntityBase<Guid>
    {
        public decimal YardPrice { get; set; }

        public string? Detail { get; set; }

        public string? DayOfWeek { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public Guid? YardTypeId { get; set; }

        public int IsDefault { get; set; }
    }
}
