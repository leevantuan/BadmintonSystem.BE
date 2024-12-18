namespace BadmintonSystem.Contract.Services.V1.ClubInformation;

public static class Request
{
    public class CreateClubInformationRequest
    {
        public string? FacebookPageLink { get; set; }

        public string? InstagramLink { get; set; }

        public string? MapLink { get; set; }

        public Guid? ClubId { get; set; }
    }

    public class UpdateClubInformationRequest
    {
        public Guid Id { get; set; }

        public string? FacebookPageLink { get; set; }

        public string? InstagramLink { get; set; }

        public string? MapLink { get; set; }

        public Guid? ClubId { get; set; }
    }
}
