using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Address;

public static class Response
{
    public class AddressResponse : EntityBase<Guid>
    {
        public string? Unit { get; set; }

        public string? Street { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? City { get; set; }

        public string? Province { get; set; }
    }

    public class AddressDetailResponse : EntityBase<Guid>
    {
        public string? Unit { get; set; }

        public string? Street { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? City { get; set; }

        public string? Province { get; set; }
    }
}
