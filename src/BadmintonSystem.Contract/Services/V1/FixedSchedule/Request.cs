namespace BadmintonSystem.Contract.Services.V1.FixedSchedule;

public static class Request
{
    public class CreateFixedScheduleRequest
    {
        public Guid? UserId { get; set; }

        public Guid YardId { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class UpdateFixedScheduleRequest
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? YardId { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class CreateFixedScheduleDetailRequest : CreateFixedScheduleRequest
    {
        public List<CreateDayOfWeekDetailRequest> DayOfWeeks { get; set; }
    }

    public class CreateDayOfWeekDetailRequest
    {
        public string WeekName { get; set; }

        public List<Guid> TimeSlotIds { get; set; }
    }
}
