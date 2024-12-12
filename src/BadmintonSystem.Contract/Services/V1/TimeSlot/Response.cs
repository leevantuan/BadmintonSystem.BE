namespace BadmintonSystem.Contract.Services.V1.TimeSlot;

public static class Response
{
    public record TimeSlotResponse(
        TimeSpan StartTime,
        TimeSpan EndTime);

    public class TimeSlotDetailResponse
    {
        public Guid Id { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }
    }
}
