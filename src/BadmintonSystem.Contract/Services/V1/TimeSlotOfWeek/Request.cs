namespace BadmintonSystem.Contract.Services.V1.TimeSlotOfWeek;

public static class Request
{
    public class CreateTimeSlotOfWeekRequest
    {
        public Guid TimeSlotId { get; set; }

        public Guid DayOfWeekId { get; set; }
    }
}
