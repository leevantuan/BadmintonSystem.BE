namespace BadmintonSystem.Contract.Services.V1.ServiceLine;

public static class Request
{
    public class CreateServiceLineRequest
    {
        public Guid? ServiceId { get; set; }

        public Guid? ComboFixedId { get; set; }

        public int? Quantity { get; set; }

        //public decimal? TotalPrice { get; set; }

        public Guid? BillId { get; set; }
    }

    public class UpdateServiceLineRequest
    {
        public Guid Id { get; set; }

        public Guid? ServiceId { get; set; }

        public Guid? ComboFixedId { get; set; }

        public int? Quantity { get; set; }

        public decimal? TotalPrice { get; set; }

        public Guid? BillId { get; set; }
    }
}
