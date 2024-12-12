namespace BadmintonSystem.Contract.Services.V1.ClubAddress;

public static class Request
{
    public record CreateClubAddressRequest(
        Guid AddressId,
        Guid ClubId,
        string Branch);

    public record DeleteClubAddressRequest(
        Guid AddressId,
        Guid ClubId);

    public class UpdateClubAddressRequest
    {
        public Guid AddressId { get; set; }

        public Guid? ClubId { get; set; }

        public string? Branch { get; set; }
    }
}
