using System.Text;
using AutoMapper;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using DayOfWeek = BadmintonSystem.Domain.Entities.DayOfWeek;

namespace BadmintonSystem.Application.UseCases.V1.Services;

public class YardPriceService(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<YardPrice, Guid> repository)
    : IYardPriceService
{
    public async Task<bool> CreateYardPrice(DateTime date, Guid userId)
    {
        var baseQueryBuilder = new StringBuilder();
        var extensionQueryBuilder = new StringBuilder();
        var cteQueryBuilder = new StringBuilder();
        var insertQueryBuilder = new StringBuilder();
        extensionQueryBuilder.Append(@"CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";");

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
	            WHERE '2025-01-13'::DATE BETWEEN fixedSchedule.""{nameof(FixedSchedule.StartDate)}"" AND fixedSchedule.""{nameof(FixedSchedule.EndDate)}""
            )");

        insertQueryBuilder.Append($@"INSERT INTO ""YardPrice"" (
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
	            @CreatedBy AS ""{nameof(YardPrice.CreatedBy)}"",
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
	            AND timeSlot.""{nameof(TimeSlot.EndTime)}"" < price.""{nameof(Price.EndTime)}""
            LEFT JOIN FixedScheduleTemp 
	        ON FixedScheduleTemp.""TimeSlotId"" = timeSlot.""Id""; ");

        baseQueryBuilder.Append(extensionQueryBuilder);
        baseQueryBuilder.Append(" \n");
        baseQueryBuilder.Append(cteQueryBuilder);
        baseQueryBuilder.Append(" \n");
        baseQueryBuilder.Append(insertQueryBuilder);

        NpgsqlParameter[] parameters =
        {
            new("@CreatedBy", userId)
        };

        context.Database.ExecuteSqlRaw(baseQueryBuilder.ToString(), parameters);

        await context.SaveChangesAsync();

        return true;
    }
}
