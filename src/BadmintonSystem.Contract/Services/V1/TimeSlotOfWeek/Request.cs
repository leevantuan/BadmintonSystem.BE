namespace BadmintonSystem.Contract.Services.V1.TimeSlotOfWeek;

public static class Request
{
    public class CreateTimeSlotOfWeekRequest
    {
        public Guid TimeSlotId { get; set; }

        public Guid TimeSlotOfWeekId { get; set; }
    }

    public class UpdateTimeSlotOfWeekRequest
    {
        public Guid TimeSlotId { get; set; }

        public Guid TimeSlotOfWeekId { get; set; }
    }
}
