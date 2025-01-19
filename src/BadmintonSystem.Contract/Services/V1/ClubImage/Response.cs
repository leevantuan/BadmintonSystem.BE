namespace BadmintonSystem.Contract.Services.V1.ClubImage;

public static class Response
{
    public class ClubImageDetailResponse
    {
        public Guid Id { get; set; }

        public string? ImageLink { get; set; }
    }
}
