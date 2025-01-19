namespace BadmintonSystem.Contract.Services.V1.ClubInformation;

public static class Response
{
    public class ClubInformationDetailResponse
    {
        public Guid Id { get; set; }

        public string? FacebookPageLink { get; set; }

        public string? InstagramLink { get; set; }

        public string? MapLink { get; set; }
    }
}
