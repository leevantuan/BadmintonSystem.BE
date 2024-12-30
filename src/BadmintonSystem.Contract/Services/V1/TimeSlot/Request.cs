namespace BadmintonSystem.Contract.Services.V1.TimeSlot;

public static class Request
{
    public class CreateTimeSlotRequest
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }

    public class UpdateTimeSlotRequest
    {
        public Guid Id { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }
    }
}
