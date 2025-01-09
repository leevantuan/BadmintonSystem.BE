using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.FixedSchedule;

public static class Response
{
    public class FixedScheduleResponse
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid YardId { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class FixedScheduleDetailResponse : EntityBase<Guid>
    {
        public Guid UserId { get; set; }

        public Guid YardId { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class GetFixedScheduleDetailResponse : FixedScheduleDetailResponse
    {
        public Yard.Response.YardResponse? Yard { get; set; }

        public List<DayOfWeekDetail>? DaysOfWeekDetails { get; set; }
    }

    public class DayOfWeekDetail : DayOfWeek.Response.DayOfWeekResponse
    {
        public List<TimeSlot.Response.TimeSlotDetailResponse>? TimeSlots { get; set; }
    }

    public class GetFixedScheduleDetailSql
    {
        // FIXED SCHEDULE
        public Guid? FixedSchedule_Id { get; set; }

        public Guid? FixedSchedule_UserId { get; set; }

        public Guid? FixedSchedule_YardId { get; set; }

        public string? FixedSchedule_PhoneNumber { get; set; }

        public DateTime? FixedSchedule_StartDate { get; set; }

        public DateTime? FixedSchedule_EndDate { get; set; }

        // YARD ID
        public Guid? Yard_Id { get; set; }

        public string? Yard_Name { get; set; }

        public Guid? Yard_YardTypeId { get; set; }

        public int? Yard_IsStatus { get; set; }

        // DAY OF WEEK
        public Guid? DayOfWeek_Id { get; set; }

        public Guid? DayOfWeek_FixedScheduleId { get; set; }

        public string? DayOfWeek_WeekName { get; set; }

        // TIME SLOT
        public Guid? TimeSlot_Id { get; set; }

        public TimeSpan? TimeSlot_StartTime { get; set; }

        public TimeSpan? TimeSlot_EndTime { get; set; }
    }
}
