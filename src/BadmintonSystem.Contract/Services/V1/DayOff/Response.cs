using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.DayOff;

public static class Response
{
    public class DayOffResponse
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string Content { get; set; }
    }

    public class DayOffDetailResponse : EntityBase<Guid>
    {
        public DateTime Date { get; set; }

        public string Content { get; set; }
    }
}
