using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.FixedSchedule;

public static class Response
{
    public class FixedScheduleResponse
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class FixedScheduleDetailResponse : EntityBase<Guid>
    {
        public Guid UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
