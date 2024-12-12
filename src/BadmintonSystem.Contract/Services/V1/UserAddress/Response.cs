namespace BadmintonSystem.Contract.Services.V1.UserAddress;

public static class Response
{
    public record UserAddressResponse(
        Guid AddressId,
        Guid UserId,
        int IsDefault);

    public class UserAddressDetailResponse
    {
        public Guid AddressId { get; set; }

        public Guid? UserId { get; set; }

        public int? IsDefault { get; set; }
    }
}
