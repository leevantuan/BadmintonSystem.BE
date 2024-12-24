namespace BadmintonSystem.Contract.Services.V1.TimeSlotOfWeek;

public static class Request
{
    public record CreateTimeSlotOfWeekRequest(
        Guid TimeSlotId,
        Guid TimeSlotOfWeekId);

    public class UpdateTimeSlotOfWeekRequest
    {
        public Guid TimeSlotId { get; set; }

        public Guid TimeSlotOfWeekId { get; set; }
    }
}
