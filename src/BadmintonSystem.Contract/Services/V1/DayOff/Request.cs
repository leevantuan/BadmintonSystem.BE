namespace BadmintonSystem.Contract.Services.V1.DayOff;

public static class Request
{
    public record CreateDayOffRequest(
        DateTime Date,
        string Content);

    public class UpdateDayOffRequest
    {
        public Guid Id { get; set; }

        public DateTime? Date { get; set; }

        public string? Content { get; set; }
    }
}
