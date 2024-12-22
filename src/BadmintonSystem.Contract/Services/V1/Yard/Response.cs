using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Yard;

public static class Response
{
    public record YardResponse(Guid Id, string Name, Guid YardTypeId, int IsStatus);

    public class YardDetailResponse : EntityAuditBase<Guid>
    {
        public string? Name { get; set; }

        public Guid YardTypeId { get; set; }

        public int IsStatus { get; set; }
    }
}
