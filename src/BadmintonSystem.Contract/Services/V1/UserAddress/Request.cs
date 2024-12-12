namespace BadmintonSystem.Contract.Services.V1.UserAddress;

public static class Request
{
    public record CreateUserAddressRequest(
        Guid AddressId,
        Guid UserId,
        int IsDefault);

    public record DeleteUserAddressRequest(
        Guid AddressId,
        Guid UserId);

    public class UpdateUserAddressRequest
    {
        public Guid AddressId { get; set; }

        public Guid? UserId { get; set; }

        public int? IsDefault { get; set; }
    }
}
