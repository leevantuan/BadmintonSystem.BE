namespace BadmintonSystem.Contract.Services.V1.BillLine;

public static class Request
{
    public class CreateBillLineRequest
    {
        public Guid BillId { get; set; }

        public Guid? YardId { get; set; }

        public TimeSpan? StartTime { get; set; }
    }

    public class UpdateBillLineRequest
    {
        public Guid Id { get; set; }

        public TimeSpan? EndTime { get; set; }
    }

    public class UpdateQuantityServiceRequest
    {
        public Guid ServiceLineId { get; set; }

        public decimal Quantity { get; set; }
    }
}
