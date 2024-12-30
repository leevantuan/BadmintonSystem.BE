namespace BadmintonSystem.Contract.Services.V1.DayOfWeek;

public static class Request
{
    public class CreateDayOfWeekRequest
    {
        public Guid? FixedScheduleId { get; set; }

        public string? WeekName { get; set; }
    }

    public class UpdateDayOfWeekRequest
    {
        public Guid Id { get; set; }

        public Guid? FixedScheduleId { get; set; }

        public string? WeekName { get; set; }
    }
}
