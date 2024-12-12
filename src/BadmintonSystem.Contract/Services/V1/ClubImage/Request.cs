namespace BadmintonSystem.Contract.Services.V1.ClubImage;

public static class Request
{
    public record CreateClubImageRequest(
        string ImageLink,
        Guid ClubId);

    public class UpdateClubImageRequest
    {
        public Guid Id { get; set; }

        public string? ImageLink { get; set; }

        public Guid? ClubId { get; set; }
    }
}
