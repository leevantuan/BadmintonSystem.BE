namespace BadmintonSystem.Contract.Services.V1.TimeSlotOfWeek;

public static class Response
{
    public record TimeSlotOfWeekResponse(
        Guid TimeSlotId,
        Guid TimeSlotOfWeekId);

    public class TimeSlotOfWeekDetailResponse
    {
        public Guid TimeSlotId { get; set; }

        public Guid TimeSlotOfWeekId { get; set; }
    }
}
