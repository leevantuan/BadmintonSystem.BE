namespace BadmintonSystem.Contract.Services.V1.TimeSlot;

public static class Response
{
    public class TimeSlotResponse
    {
        public Guid Id { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public Guid YardId { get; set; }
    }

    public class TimeSlotDetailResponse
    {
        public Guid Id { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public Guid? YardId { get; set; }
    }
}
