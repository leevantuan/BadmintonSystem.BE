using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Yard;

public static class Response
{
    public class YardResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? YardTypeId { get; set; }

        public int? IsStatus { get; set; }
    }

    public class YardDetailResponse : EntityAuditBase<Guid>
    {
        public string? Name { get; set; }

        public Guid YardTypeId { get; set; }

        public int IsStatus { get; set; }
    }
}
