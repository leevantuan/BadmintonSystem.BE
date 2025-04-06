using System.Runtime.CompilerServices;
using System.Text;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.YardPrice;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

    public async Task<List<Response.YardPricesByDateDetailResponse>> GetYardPrices(DateTime date, string tenant, Guid userId, CancellationToken cancellationToken)
    {
        string endpoint = $"{tenant}-get-yard-prices-by-date";

        string cacheKey = StringExtension.GenerateCacheKeyFromRequest(endpoint, date.Date);

        string cacheData = await redisService.GetAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheData))
        {
            List<Response.YardPricesByDateDetailResponse>? data =
                JsonConvert.DeserializeObject<List<Response.YardPricesByDateDetailResponse>>(cacheData);
            return data;
        }

        IQueryable<Domain.Entities.YardPrice>? effectiveDateIsExists =
            context.YardPrice.Where(x => x.EffectiveDate.Date == date.Date);

        if (!effectiveDateIsExists.Any())
        {
            await CreateYardPrice(date, userId, cancellationToken);
        }

        string yardColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Yard,
                Contract.Services.V1.Yard.Response.YardResponse>();

        string yardPriceColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.YardPrice,
                Response.YardPriceResponse>();

        string yardTypeColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.YardType,
                Contract.Services.V1.YardType.Response.YardTypeResponse>();

        string priceColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Price,
                Contract.Services.V1.Price.Response.PriceResponse>();

        string timeSlotColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.TimeSlot,
                Contract.Services.V1.TimeSlot.Response.TimeSlotResponse>();

        DateTime filterDate = date.Date;

        var baseQueryBuilder = new StringBuilder();
        baseQueryBuilder.Append(
            $@"FROM ""{nameof(Domain.Entities.YardPrice)}"" AS yardPrice
                LEFT JOIN ""{nameof(Domain.Entities.Price)}"" AS price
                ON yardPrice.""{nameof(Domain.Entities.YardPrice.PriceId)}"" = price.""{nameof(Domain.Entities.Price.Id)}""
                AND price.""{nameof(Domain.Entities.Price.IsDeleted)}"" = false
                JOIN ""{nameof(Domain.Entities.Yard)}"" AS yard
                ON yard.""{nameof(Domain.Entities.Yard.Id)}"" = yardPrice.""{nameof(Domain.Entities.YardPrice.YardId)}""
                AND yard.""{nameof(Domain.Entities.Yard.IsDeleted)}"" = false
                JOIN ""{nameof(Domain.Entities.YardType)}"" AS yardType
                ON yardType.""{nameof(Domain.Entities.YardType.Id)}"" = yard.""{nameof(Domain.Entities.Yard.YardTypeId)}""
                AND yardType.""{nameof(Domain.Entities.YardType.IsDeleted)}"" = false
                JOIN ""{nameof(Domain.Entities.TimeSlot)}"" AS timeSlot
                ON timeSlot.""{nameof(Domain.Entities.TimeSlot.Id)}"" = yardPrice.""{nameof(Domain.Entities.YardPrice.TimeSlotId)}""
                AND timeSlot.""{nameof(Domain.Entities.TimeSlot.IsDeleted)}"" = false
                WHERE yardPrice.""{nameof(Domain.Entities.YardPrice.IsDeleted)}"" = false 
                AND yardPrice.""{nameof(Domain.Entities.YardPrice.EffectiveDate)}""::DATE = '{filterDate}'");

        var yardPriceQueryBuilder = new StringBuilder();
        yardPriceQueryBuilder.Append(
            $@"SELECT {yardColumns}, {yardPriceColumns}, {yardTypeColumns}, {priceColumns}, {timeSlotColumns} ");
        yardPriceQueryBuilder.Append(" \n");
        yardPriceQueryBuilder.Append(baseQueryBuilder.ToString());

        List<Response.YardPriceDetailSql> queryResult = await context.Database.SqlQuery<Response.YardPriceDetailSql>(
                FormattableStringFactory.Create(yardPriceQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        // Group by
        var results = queryResult.GroupBy(p => p.Yard_Id)
            .Select(g => new Response.YardPricesByDateDetailResponse
            {
                Yard = g.Where(x => x.Yard_Id != null)
                    .Select(s => new Contract.Services.V1.Yard.Response.YardResponse
                    {
                        Id = s.Yard_Id ?? Guid.Empty,
                        Name = s.Yard_Name ?? string.Empty,
                        YardTypeId = s.Yard_YardTypeId ?? Guid.Empty,
                        IsStatus = s.Yard_IsStatus ?? 0
                    })
                    .DistinctBy(s => s.Id).OrderBy(x => x.Name)
                    .FirstOrDefault(),

                YardPricesDetails = g.Select(x => new Response.YardPricesByDateDetail
                {
                    Id = x.YardPrice_Id ?? Guid.Empty,
                    YardId = x.YardPrice_YardId ?? Guid.Empty,
                    TimeSlotId = x.YardPrice_TimeSlotId ?? Guid.Empty,
                    PriceId = x.YardPrice_PriceId,
                    EffectiveDate = x.YardPrice_EffectiveDate ?? DateTime.Now,
                    IsBooking = x.YardPrice_IsBooking ?? 0,
                    Price = x.Price_YardPrice ?? 0,
                    StartTime = x.TimeSlot_StartTime ?? TimeSpan.Zero,
                    EndTime = x.TimeSlot_EndTime ?? TimeSpan.Zero
                }).OrderBy(x => x.StartTime).ToList()
            }).OrderBy(x => x.Yard.Name)
            .ToList();

        return results;
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
