using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Club;

public static class Response
{
    public record ClubResponse(
        Guid Id,
        string Name,
        string Hotline,
        TimeSpan OpeningTime,
        TimeSpan ClosingTime,
        string Code);

    public class ClubDetailResponse : EntityAuditBase<Guid>
    {
        public string? Name { get; set; }

        public string? Hotline { get; set; }

        public TimeSpan? OpeningTime { get; set; }

        public TimeSpan? ClosingTime { get; set; }

        public string? Code { get; set; }
    }
}
