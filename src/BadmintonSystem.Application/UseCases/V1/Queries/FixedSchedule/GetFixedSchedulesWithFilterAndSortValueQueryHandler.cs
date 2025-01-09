using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.FixedSchedule;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using DayOfWeek = BadmintonSystem.Domain.Entities.DayOfWeek;
using Response = BadmintonSystem.Contract.Services.V1.FixedSchedule.Response;

namespace BadmintonSystem.Application.UseCases.V1.Queries.FixedSchedule;

public sealed class GetFixedSchedulesWithFilterAndSortValueQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.FixedSchedule, Guid> fixedScheduleRepository)
    : IQueryHandler<Query.GetFixedSchedulesWithFilterAndSortValueQuery,
        PagedResult<Response.GetFixedScheduleDetailResponse>>
{
    public async Task<Result<PagedResult<Response.GetFixedScheduleDetailResponse>>> Handle
        (Query.GetFixedSchedulesWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        // Pagination
        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.FixedSchedule>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.FixedSchedule>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.FixedSchedule>.UpperPageSize
                ? PagedResult<Domain.Entities.FixedSchedule>.UpperPageSize
                : request.Data.PageSize;

        string yardColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Yard,
                Contract.Services.V1.Yard.Response.YardResponse>();

        string dayOfWeekColumns = StringExtension
            .TransformPropertiesToSqlAliases<DayOfWeek,
                Contract.Services.V1.DayOfWeek.Response.DayOfWeekResponse>();

        string fixedScheduleColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.FixedSchedule,
                Response.FixedScheduleResponse>();

        string timeSlotColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.TimeSlot,
                Contract.Services.V1.TimeSlot.Response.TimeSlotResponse>();

        var baseQueryBuilder = new StringBuilder();
        baseQueryBuilder.Append($@"WITH fixedScheduleTemp AS (
                SELECT *
                FROM ""{nameof(Domain.Entities.FixedSchedule)}""
                WHERE ""{nameof(Domain.Entities.FixedSchedule.IsDeleted)}"" = false
                AND ""{nameof(Domain.Entities.FixedSchedule.PhoneNumber)}"" ILIKE '%{request.Data.SearchTerm}%' ");

        // FILTER MULTIPLE
        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = FixedScheduleExtension.GetSortFixedScheduleProperty(item.Key);
                baseQueryBuilder.Append($@"AND ""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    baseQueryBuilder.Append($@"'%{value}%', ");
                }

                baseQueryBuilder.Length -= 2;

                baseQueryBuilder.Append("]) ");
            }
        }

        // SORT MULTIPLE
        if (request.Data.SortColumnAndOrder.Any())
        {
            baseQueryBuilder.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = AddressExtension.GetSortAddressProperty(item.Key);
                baseQueryBuilder.Append(item.Value == SortOrder.Descending
                    ? $@" ""{key}"" DESC, "
                    : $@" ""{key}"" ASC, ");
            }

            baseQueryBuilder.Length -= 2;
        }

        baseQueryBuilder.Append($"\nOFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");
        baseQueryBuilder.Append(")\n");

        baseQueryBuilder.Append(
            $@"SELECT {yardColumns}, {timeSlotColumns}, {dayOfWeekColumns}, {fixedScheduleColumns}
                FROM fixedScheduleTemp AS fixedSchedule
                JOIN ""{nameof(DayOfWeek)}"" AS dayOfWeek
                ON dayOfWeek.""{nameof(DayOfWeek.FixedScheduleId)}"" = fixedSchedule.""{nameof(Domain.Entities.FixedSchedule.Id)}""
                AND dayOfWeek.""{nameof(DayOfWeek.IsDeleted)}"" = false
                JOIN ""{nameof(TimeSlotOfWeek)}"" AS timeSlotOfWeek
                ON timeSlotOfWeek.""{nameof(TimeSlotOfWeek.DayOfWeekId)}"" = dayOfWeek.""{nameof(DayOfWeek.Id)}""
                JOIN ""{nameof(Domain.Entities.TimeSlot)}"" AS timeSlot
                ON timeSlot.""{nameof(Domain.Entities.TimeSlot.Id)}"" = timeSlotOfWeek.""{nameof(TimeSlotOfWeek.TimeSlotId)}""
                AND timeSlot.""{nameof(Domain.Entities.TimeSlot.IsDeleted)}"" = false
                JOIN ""{nameof(Domain.Entities.Yard)}"" AS yard
                ON yard.""{nameof(Domain.Entities.Yard.Id)}"" = fixedSchedule.""{nameof(Domain.Entities.FixedSchedule.YardId)}""
                AND yard.""{nameof(Domain.Entities.Yard.IsDeleted)}"" = false");

        List<Response.GetFixedScheduleDetailSql> queryResult = await fixedScheduleRepository
            .ExecuteSqlQuery<Response.GetFixedScheduleDetailSql>(
                FormattableStringFactory.Create(baseQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        // Group by
        var results = queryResult.GroupBy(x => x.FixedSchedule_Id)
            .Select(x => new Response.GetFixedScheduleDetailResponse
            {
                Id = x.Key ?? Guid.Empty,
                UserId = x.First().FixedSchedule_UserId ?? Guid.Empty,
                YardId = x.First().FixedSchedule_YardId ?? Guid.Empty,
                PhoneNumber = x.First().FixedSchedule_PhoneNumber ?? string.Empty,
                StartDate = x.First().FixedSchedule_StartDate ?? DateTime.Now,
                EndDate = x.First().FixedSchedule_EndDate ?? DateTime.Now,

                Yard = x.Where(g => g.Yard_Id != Guid.Empty)
                    .Select(x => new Contract.Services.V1.Yard.Response.YardResponse
                    {
                        Id = x.Yard_Id ?? Guid.Empty,
                        Name = x.Yard_Name ?? string.Empty,
                        YardTypeId = x.Yard_YardTypeId ?? Guid.Empty,
                        IsStatus = x.Yard_IsStatus ?? 0
                    }).DistinctBy(x => x.Id)
                    .FirstOrDefault(),

                DaysOfWeekDetails = queryResult.GroupBy(x => new { x.FixedSchedule_Id, x.DayOfWeek_Id })
                    .Select(x => new Response.DayOfWeekDetail
                    {
                        Id = x.First().DayOfWeek_Id ?? Guid.Empty,
                        FixedScheduleId = x.First().FixedSchedule_Id ?? Guid.Empty,
                        WeekName = x.First().DayOfWeek_WeekName ?? string.Empty,

                        TimeSlots = x.Where(g => g.TimeSlot_Id != null)
                            .Select(g => new Contract.Services.V1.TimeSlot.Response.TimeSlotDetailResponse
                            {
                                Id = g.TimeSlot_Id ?? Guid.Empty,
                                StartTime = g.TimeSlot_StartTime ?? TimeSpan.Zero,
                                EndTime = g.TimeSlot_EndTime ?? TimeSpan.Zero
                            }).DistinctBy(g => g.Id).ToList()
                    }).DistinctBy(x => x.Id).ToList()
            }).ToList();

        var pagedResult =
            PagedResult<Response.GetFixedScheduleDetailResponse>.Create(
                results,
                pageIndex,
                pageSize,
                results.Count());

        return Result.Success(pagedResult);
    }
}
