using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.BillLine;

public static class Response
{
    public class BillLineResponse : EntityBase<Guid>
    {
        public Guid BillId { get; set; }

        public Guid? YardId { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public decimal? TotalPrice { get; set; }

        public int? IsActive { get; set; }
    }

    public class BillLineDetailResponse : EntityAuditBase<Guid>
    {
        public Guid BillId { get; set; }

        public Guid? YardId { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public decimal? TotalPrice { get; set; }
    }

    public class BillLineDetail
    {
        public BillLineResponse BillLine { get; set; }

        public Yard.Response.YardResponse Yard { get; set; }

        public Price.Response.PriceResponse Price { get; set; }
    }
}
