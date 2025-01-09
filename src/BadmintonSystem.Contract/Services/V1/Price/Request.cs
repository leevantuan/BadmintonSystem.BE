namespace BadmintonSystem.Contract.Services.V1.Price;

public static class Request
{
    public record CreatePriceRequest(
        decimal YardPrice,
        string? Detail,
        string? DayOfWeek,
        TimeSpan? StartTime,
        TimeSpan? EndTime,
        Guid? YardTypeId,
        int IsDefault);

    public class UpdatePriceRequest
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
}
