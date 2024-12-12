namespace BadmintonSystem.Contract.Services.V1.Club;

public static class Request
{
    public record CreateClubRequest(
        string Name,
        string Hotline,
        TimeSpan OpeningTime,
        TimeSpan ClosingTime,
        string Code);

    public class UpdateClubRequest
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Hotline { get; set; }

        public TimeSpan? OpeningTime { get; set; }

        public TimeSpan? ClosingTime { get; set; }

        public string? Code { get; set; }
    }
}
