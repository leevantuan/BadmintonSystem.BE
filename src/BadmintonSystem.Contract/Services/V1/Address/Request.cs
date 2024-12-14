namespace BadmintonSystem.Contract.Services.V1.Address;

public static class Request
{
    public record CreateAddressRequest(
        string Unit,
        string Street,
        string AddressLine1,
        string AddressLine2,
        string Province,
        string City);

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
}
