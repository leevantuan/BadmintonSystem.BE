namespace BadmintonSystem.Contract.Services.V1.DayOfWeek;

public static class Request
{
    public class CreateDayOfWeekRequest
    {
        public Guid? FixedScheduleId { get; set; }

        public string? WeekName { get; set; }
    }
}
