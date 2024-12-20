namespace BadmintonSystem.Contract.Services.V1.Address;

public static class Request
{
    public class CreateAddressRequest
    {
        public string? Unit { get; set; }

        public string? Street { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? City { get; set; }

        public string? Province { get; set; }
    }

    public class UpdateAddressRequest
    {
        public Guid Id { get; set; }

        public string? Unit { get; set; }

        public string? Street { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? City { get; set; }

        public string? Province { get; set; }
    }

    // REQUEST ADDRESS BY USERID
    public class UpdateAddressByUserIdRequest : UpdateAddressRequest
    {
        public int IsDefault { get; set; }
    }
}
