using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Provider;

public static class Response
{
    public class ProviderResponse : EntityBase<Guid>
    {
        public string Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string Address { get; set; }
    }

    public class ProviderDetailResponse : EntityAuditBase<Guid>
    {
        public string Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}
