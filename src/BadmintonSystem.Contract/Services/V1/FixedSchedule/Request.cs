namespace BadmintonSystem.Contract.Services.V1.FixedSchedule;

public static class Request
{
    public record CreateFixedScheduleRequest(
        Guid UserId,
        DateTime StartDate,
        DateTime EndDate);

    public class UpdateFixedScheduleRequest
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
