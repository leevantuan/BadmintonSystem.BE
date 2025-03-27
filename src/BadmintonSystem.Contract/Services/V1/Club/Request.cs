namespace BadmintonSystem.Contract.Services.V1.Club;

public static class Request
{
    public class CreateClubRequest
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Hotline { get; set; }

        public TimeSpan? OpeningTime { get; set; }

        public TimeSpan? ClosingTime { get; set; }

        public string? Code { get; set; }
    }

    public class UpdateClubRequest
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Hotline { get; set; }

        public TimeSpan? OpeningTime { get; set; }

        public TimeSpan? ClosingTime { get; set; }

        public string? Code { get; set; }
    }

    public class CreateClubDetailsRequest : CreateClubRequest
    {
        public ClubInformation.Request.CreateClubInformationRequest ClubInformation { get; set; }

        public List<ClubImage.Request.CreateClubImageRequest> ClubImages { get; set; }

        public Address.Request.CreateAddressRequest ClubAddress { get; set; }
    }

    public class UpdateClubDetailsRequest : UpdateClubRequest
    {
        public ClubInformation.Request.UpdateClubInformationRequest ClubInformation { get; set; }

        public List<ClubImage.Request.UpdateClubImageRequest> ClubImages { get; set; }

        public Address.Request.UpdateAddressRequest ClubAddress { get; set; }
    }
}
