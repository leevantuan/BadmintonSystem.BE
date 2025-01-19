using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Sale;

public static class Response
{
    public record SaleResponse(Guid Id, string Name, int Percent, DateTime StartDate, DateTime EndDate, int IsActive);

    public class SaleDetailResponse : EntityAuditBase<Guid>
    {
        public string? Name { get; set; }

        public int? Percent { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? IsActive { get; set; }
    }
}
