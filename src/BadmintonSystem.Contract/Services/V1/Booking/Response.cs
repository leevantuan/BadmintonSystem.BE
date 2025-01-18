using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Booking;

public static class Response
{
    public record BookingResponse(
        Guid Id,
        DateTime BookingDate,
        decimal BookingTotal,
        int PaymentStatus,
        int BookingStatus);

    public class BookingDetail : EntityBase<Guid>
    {
        public DateTime? BookingDate { get; set; }

        public decimal? BookingTotal { get; set; }

        public decimal? OriginalPrice { get; set; }

        public Guid? UserId { get; set; }

        public Guid? SaleId { get; set; }

        public int? BookingStatus { get; set; }

        public int? PaymentStatus { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class GetBookingDetailResponse : BookingDetail
    {
        public DateTime EffectiveDate { get; set; }

        public UserWithBooking? User { get; set; }

        public List<BookingLineDetail> BookingLines { get; set; }
    }

    public class BookingLineDetail
    {
        public string YardName { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public decimal Price { get; set; }
    }

    public class UserWithBooking
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class GetIdsByDate
    {
        public DateTime Date { get; set; }

        public List<Guid> Ids { get; set; }
    }

    public class GetBookingDetailSql
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

        // Yard
        public Guid? Yard_Id { get; set; }

        public string? Yard_Name { get; set; }

        public Guid? Yard_YardTypeId { get; set; }

        public int? Yard_IsStatus { get; set; }

        // Yard Price
        public Guid? YardPrice_Id { get; set; }

        public DateTime? YardPrice_EffectiveDate { get; set; }

        // Time Slot
        public Guid? TimeSlot_Id { get; set; }

        public TimeSpan? TimeSlot_StartTime { get; set; }

        public TimeSpan? TimeSlot_EndTime { get; set; }

        // Booking Line
        public Guid? BookingLine_Id { get; set; }

        public Guid? BookingLine_YardPriceId { get; set; }

        public decimal? BookingLine_TotalPrice { get; set; }
    }
}
