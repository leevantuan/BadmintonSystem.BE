using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Bill;

public static class Response
{
    public class BillResponse : EntityBase<Guid>
    {
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

    public class BillDetail : EntityAuditBase<Guid>
    {
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

    public class BillDetailResponse : BillDetail
    {
        public Booking.Response.GetBookingDetailResponse? BookingDetail { get; set; }

        public List<BillLine.Response.BillLineDetail>? BillLineDetails { get; set; }
    }

    public class YardIds
    {
        public List<Guid>? Ids { get; set; }
    }

    public class GetTotalPriceSql
    {
        public Guid Id { get; set; }

        public decimal BillLine_TotalPrice { get; set; }

        public decimal ServiceLine_TotalPrice { get; set; }

        public decimal Booking_TotalPrice { get; set; }

        public decimal Booking_OriginalPrice { get; set; }

        public int Booking_PercentPrePay { get; set; }
    }
}
