namespace BadmintonSystem.Contract.Services.V1.ClubAddress;

public static class Response
{
    public record ClubAddressResponse(
        Guid AddressId,
        Guid ClubId,
        string Branch);
}
