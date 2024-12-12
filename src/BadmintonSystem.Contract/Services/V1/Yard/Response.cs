using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Yard;

public static class Response
{
    public record YardResponse(Guid Id, string Name, Guid YardTypeId);

    public class YardDetailResponse : EntityAuditBase<Guid>
    {
        public string? Name { get; set; }

        public Guid YardTypeId { get; set; }
    }
}
