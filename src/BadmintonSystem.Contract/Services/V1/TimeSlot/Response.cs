namespace BadmintonSystem.Contract.Services.V1.TimeSlot;

public static class Response
{
    public class TimeSlotResponse
    {
        public Guid Id { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }

    public class TimeSlotWithYardPriceResponse
    {
        public Guid Id { get; set; }

        public Guid YardPriceId { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }

    public class TimeSlotDetailResponse
    {
        public Guid Id { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }
    }
}
