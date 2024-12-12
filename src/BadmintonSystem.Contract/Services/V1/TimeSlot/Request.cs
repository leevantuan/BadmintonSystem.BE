namespace BadmintonSystem.Contract.Services.V1.TimeSlot;

public static class Request
{
    public record CreateTimeSlotRequest(
        TimeSpan StartTime,
        TimeSpan EndTime);

    public class UpdateTimeSlotRequest
    {
        public Guid Id { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }
    }
}
