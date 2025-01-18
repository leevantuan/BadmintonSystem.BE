namespace BadmintonSystem.Contract.Services.V1.Bill;

public static class Request
{
    public class CreateBillRequest
    {
        public decimal? TotalPrice { get; set; }

        public decimal? TotalPayment { get; set; }

        public string? Content { get; set; }

        public string? Name { get; set; }

        public Guid? UserId { get; set; }

        //public Guid? BookingId { get; set; }

        public Guid? YardId { get; set; }

        //public int? Status { get; set; }
    }

    public class UpdateBillRequest
    {
        public Guid? Id { get; set; }

        public decimal? TotalPrice { get; set; }

        public decimal? TotalPayment { get; set; }

        public string? Content { get; set; }

        public string? Name { get; set; }

        public Guid? UserId { get; set; }

        public Guid? BookingId { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int? Status { get; set; }
    }

    public class FilterBillRequest
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
