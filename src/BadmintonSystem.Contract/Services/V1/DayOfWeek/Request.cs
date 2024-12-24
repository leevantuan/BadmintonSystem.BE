namespace BadmintonSystem.Contract.Services.V1.DayOfWeek;

public static class Request
{
    public record CreateDayOfWeekRequest(
        Guid FixedScheduleId,
        string WeekName);

    public class UpdateDayOfWeekRequest
    {
        public Guid Id { get; set; }

        public Guid? FixedScheduleId { get; set; }

        public string? WeekName { get; set; }
    }
}
