namespace BadmintonSystem.Contract.Services.V1.ClubAddress;

public static class Response
{
    public record ClubAddressResponse(
        Guid AddressId,
        Guid ClubId,
        string Branch);

    public class ClubAddressDetailResponse
    {
        public Guid AddressId { get; set; }

        public Guid? ClubId { get; set; }

        public string? Branch { get; set; }
    }
}
