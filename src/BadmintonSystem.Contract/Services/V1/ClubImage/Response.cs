namespace BadmintonSystem.Contract.Services.V1.ClubImage;

public static class Response
{
    public record ClubImageResponse(
        Guid Id,
        string ImageLink,
        Guid ClubId);

    public class ClubImageDetailResponse
    {
        public Guid Id { get; set; }

        public string? ImageLink { get; set; }
    }
}
