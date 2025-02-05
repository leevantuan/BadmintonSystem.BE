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

        public int? Status { get; set; }
    }

    public class BillDetailResponse : BillDetail
    {
        public Booking.Response.BookingDetail? Booking { get; set; }

        public List<BillLine.Response.BillLineDetail>? BillLineDetails { get; set; }

        public List<ServiceLine.Response.ServiceLineDetail>? ServiceLineDetails { get; set; }

        public int? TotalPriceByRangeDate { get; set; }
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

    public class GetBillDetailsSql : GetBillSql
    {
        // Booking
        public Guid? Booking_Id { get; set; }

        public DateTime? Booking_BookingDate { get; set; }

        public decimal? Booking_BookingTotal { get; set; }

        public decimal? Booking_OriginalPrice { get; set; }

        public Guid? Booking_UserId { get; set; }

        public Guid? Booking_SaleId { get; set; }

        public int? Booking_BookingStatus { get; set; }

        public int? Booking_PaymentStatus { get; set; }

        public string? Booking_FullName { get; set; }

        public string? Booking_PhoneNumber { get; set; }

        // Service Line 
        public Guid? ServiceLine_Id { get; set; }

        public Guid? ServiceLine_ServiceId { get; set; }

        public Guid? ServiceLine_ComboFixedId { get; set; }

        public int? ServiceLine_Quantity { get; set; }

        public decimal? ServiceLine_TotalPrice { get; set; }

        public Guid? ServiceLine_BillId { get; set; }

        // Service
        public Guid? Service_Id { get; set; }

        public string? Service_Name { get; set; }

        public decimal? Service_PurchasePrice { get; set; }

        public decimal? Service_SellingPrice { get; set; }

        public decimal? Service_QuantityInStock { get; set; }

        public string? Service_Unit { get; set; }

        public Guid? Service_CategoryId { get; set; }

        public decimal? Service_QuantityPrinciple { get; set; }

        public Guid? Service_OriginalQuantityId { get; set; }

        // Bill Line
        public Guid? BillLine_Id { get; set; }

        public Guid? BillLine_BillId { get; set; }

        public Guid? BillLine_YardId { get; set; }

        public TimeSpan? BillLine_StartTime { get; set; }

        public TimeSpan? BillLine_EndTime { get; set; }

        public decimal? BillLine_TotalPrice { get; set; }

        public int? BillLine_IsActive { get; set; }

        // Yard
        public Guid? Yard_Id { get; set; }

        public string? Yard_Name { get; set; }

        public Guid? Yard_YardTypeId { get; set; }

        public int? Yard_IsStatus { get; set; }

        // Price
        public Guid? Price_Id { get; set; }

        public decimal? Price_YardPrice { get; set; }
    }

    public class GetBillSql
    {
        // Bill
        public Guid? Bill_Id { get; set; }

        public decimal? Bill_TotalPrice { get; set; }

        public decimal? Bill_TotalPayment { get; set; }

        public string? Bill_Content { get; set; }

        public string? Bill_Name { get; set; }

        public Guid? Bill_UserId { get; set; }

        public Guid? Bill_BookingId { get; set; }

        public int? Bill_Status { get; set; }
    }

    public class BookingHubResponse
    {
        public List<Guid> Ids { get; set; }

        public string Type { get; set; }
    }

    public class BookingReserveHubResponse
    {
        public Guid Id { get; set; }

        public string Type { get; set; }
    }
}
