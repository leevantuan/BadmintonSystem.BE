namespace BadmintonSystem.Contract.Services.V2.Club;
public static class Response
{
    public record ClubResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? HotLine { get; set; }
        public string? FacebookPageLink { get; set; }
        public string? InstagramPageLink { get; set; }
        public string? MapLink { get; set; }
        public string? ImageLink { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? ClosingTime { get; set; }
    }
}
