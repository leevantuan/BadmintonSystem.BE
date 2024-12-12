using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Address;

public static class Response
{
    public record AddressResponse(
        Guid Id,
        string? Unit,
        string? Street,
        string? AddressLine1,
        string? AddressLine2,
        string? City);

    public class AddressDetailResponse : EntityAuditBase<Guid>
    {
        public string? Unit { get; set; }

        public string? Street { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? City { get; set; }
    }
}
