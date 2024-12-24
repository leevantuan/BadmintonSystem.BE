using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.DayOfWeek;

public static class Response
{
    public class DayOfWeekResponse
    {
        public Guid Id { get; set; }

        public Guid FixedScheduleId { get; set; }

        public string WeekName { get; set; }
    }

    public class DayOfWeekDetailResponse : EntityBase<Guid>
    {
        public Guid FixedScheduleId { get; set; }

        public string WeekName { get; set; }
    }
}
