using System.Text;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using DayOfWeek = BadmintonSystem.Domain.Entities.DayOfWeek;

namespace BadmintonSystem.Application.UseCases.V1.Services;

public sealed class YardPriceService(
    IRedisService redisService,
    ApplicationDbContext context)
    : IYardPriceService
{
    public async Task<bool> CreateYardPrice(DateTime date, Guid userId, CancellationToken cancellationToken)
    {
        var extensionQueryBuilder = new StringBuilder();
        extensionQueryBuilder.Append(@"CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";");

        var baseQueryBuilder = new StringBuilder();
        baseQueryBuilder.Append(extensionQueryBuilder);
        baseQueryBuilder.Append(" \n");
        baseQueryBuilder.Append(CteQueryBuilder(date));
        baseQueryBuilder.Append(" \n");
        baseQueryBuilder.Append(InsertQueryBuilder(date, userId));

        await context.Database.ExecuteSqlRawAsync(baseQueryBuilder.ToString(), cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task UpdateYardPricesByBookingId(Guid bookingId, CancellationToken cancellationToken)
    {
        var updateYardPriceQueryBuilder = new StringBuilder();
        updateYardPriceQueryBuilder.Append($@"WITH yardPriceIds AS (
	        SELECT ""{nameof(BookingLine.YardPriceId)}"" AS ""Id""
	            FROM ""{nameof(BookingLine)}"" AS bookingLine
	            WHERE ""{nameof(BookingLine.BookingId)}"" = '{bookingId}'
	            AND bookingLine.""{nameof(BookingLine.IsDeleted)}"" = false
            )
            UPDATE ""{nameof(YardPrice)}"" 
            SET ""{nameof(YardPrice.IsBooking)}"" = {(int)BookingEnum.UNBOOKED},
	            ""{nameof(YardPrice.ModifiedDate)}"" = NOW()
            WHERE ""{nameof(YardPrice.Id)}"" IN (SELECT ""Id"" FROM yardPriceIds)");

        await context.Database.ExecuteSqlRawAsync(updateYardPriceQueryBuilder.ToString(), cancellationToken);
    }

    private string CteQueryBuilder(DateTime date)
    {
        var cteQueryBuilder = new StringBuilder();
        cteQueryBuilder.Append($@"WITH FixedScheduleTemp AS (
	            SELECT 
		            fixedSchedule.""{nameof(FixedSchedule.StartDate)}"", 
		            fixedSchedule.""{nameof(FixedSchedule.EndDate)}"", 
		            UPPER(dayOfWeek.""{nameof(DayOfWeek.WeekName)}"") AS ""WeekName"", 
		            timeSlotOfWeek.""{nameof(TimeSlotOfWeek.TimeSlotId)}"",
		            fixedSchedule.""{nameof(FixedSchedule.YardId)}"" AS ""YardId""
	            FROM ""{nameof(FixedSchedule)}"" AS fixedSchedule 
	            LEFT JOIN ""{nameof(DayOfWeek)}"" AS dayOfWeek 
		        ON dayOfWeek.""{nameof(DayOfWeek.FixedScheduleId)}"" = fixedSchedule.""{nameof(FixedSchedule.Id)}""
	            LEFT JOIN ""{nameof(TimeSlotOfWeek)}"" AS timeSlotOfWeek 
		        ON timeSlotOfWeek.""{nameof(TimeSlotOfWeek.DayOfWeekId)}"" = dayOfWeek.""Id""
	            WHERE '{date}'::DATE BETWEEN fixedSchedule.""{nameof(FixedSchedule.StartDate)}"" AND fixedSchedule.""{nameof(FixedSchedule.EndDate)}""
            )");

        return cteQueryBuilder.ToString();
    }

    private string InsertQueryBuilder(DateTime date, Guid userId)
    {
        var insertQueryBuilder = new StringBuilder();
        insertQueryBuilder.Append($@"INSERT INTO ""{nameof(YardPrice)}"" (
	            ""{nameof(YardPrice.Id)}"", 
	            ""{nameof(YardPrice.YardId)}"", 
	            ""{nameof(YardPrice.TimeSlotId)}"",
	            ""{nameof(YardPrice.PriceId)}"", 
	            ""{nameof(YardPrice.EffectiveDate)}"",
	            ""{nameof(YardPrice.IsBooking)}"",
	            ""{nameof(YardPrice.CreatedDate)}"", 
	            ""{nameof(YardPrice.ModifiedDate)}"", 
	            ""{nameof(YardPrice.CreatedBy)}"", 
	            ""{nameof(YardPrice.ModifiedBy)}"", 
	            ""{nameof(YardPrice.IsDeleted)}"", 
	            ""{nameof(YardPrice.DeletedAt)}"") 
            SELECT uuid_generate_v4() AS ""{nameof(YardPrice.Id)}"", 
	            yard.""{nameof(Yard.Id)}"", 
	            timeSlot.""{nameof(TimeSlot.Id)}"", 
	            price.""{nameof(Price.Id)}"",
	            '{date}'::DATE AS ""{nameof(YardPrice.EffectiveDate)}"", 
	            CASE 
		            WHEN FixedScheduleTemp.""WeekName"" = UPPER(TO_CHAR('{date}'::DATE, 'FMDay'))
			            AND FixedScheduleTemp.""TimeSlotId"" = timeSlot.""{nameof(TimeSlot.Id)}""
			            AND FixedScheduleTemp.""YardId"" = yard.""{nameof(Yard.Id)}""
		            THEN 1 
		            ELSE 0 
	            END AS ""{nameof(YardPrice.IsBooking)}"",
	            NOW() AS ""{nameof(YardPrice.CreatedDate)}"", 
	            NULL AS ""{nameof(YardPrice.ModifiedDate)}"", 
	            '{userId}' AS ""{nameof(YardPrice.CreatedBy)}"",
	            NULL AS ""{nameof(YardPrice.ModifiedBy)}"", 
	            FALSE AS ""{nameof(YardPrice.IsDeleted)}"", 
	            NULL AS ""{nameof(YardPrice.DeletedAt)}"" 
            FROM ""{nameof(Yard)}"" AS yard
            CROSS JOIN ""{nameof(TimeSlot)}"" AS timeSlot
            LEFT JOIN ""{nameof(YardType)}"" AS yardType
	        ON yardType.""{nameof(YardType.Id)}"" = yard.""{nameof(Yard.YardTypeId)}""
            LEFT JOIN ""{nameof(Price)}"" AS price 
	        ON price.""{nameof(Price.YardTypeId)}"" = yardType.""{nameof(YardType.Id)}""
	            AND UPPER(TO_CHAR('{date}'::DATE, 'FMDay')) = price.""{nameof(Price.DayOfWeek)}""
	            AND timeSlot.""{nameof(TimeSlot.StartTime)}"" >= price.""{nameof(Price.StartTime)}""
	            AND timeSlot.""{nameof(TimeSlot.StartTime)}"" < price.""{nameof(Price.EndTime)}""
            LEFT JOIN FixedScheduleTemp 
	        ON FixedScheduleTemp.""TimeSlotId"" = timeSlot.""Id""; ");

        return insertQueryBuilder.ToString();
    }
}
