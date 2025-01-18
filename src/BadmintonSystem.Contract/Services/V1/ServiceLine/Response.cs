using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.ServiceLine;

public static class Response
{
    public class ServiceLineResponse : EntityBase<Guid>
    {
        public Guid? ServiceId { get; set; }

        public Guid? ComboFixedId { get; set; }

        public int? Quantity { get; set; }

        public decimal? TotalPrice { get; set; }

        public Guid? BillId { get; set; }
    }

    public class ServiceLineDetailResponse : EntityAuditBase<Guid>
    {
        public Guid? ServiceId { get; set; }

        public Guid? ComboFixedId { get; set; }

        public int? Quantity { get; set; }

        public decimal? TotalPrice { get; set; }

        public Guid? BillId { get; set; }
    }

    public class ServiceLineDetail
    {
        public ServiceLineResponse ServiceLine { get; set; }

        public Service.Response.ServiceResponse Service { get; set; }
    }
}
