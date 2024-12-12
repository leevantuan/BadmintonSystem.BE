using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.TimeSlot;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.TimeSlot;

public sealed class GetTimeSlotsWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.TimeSlot, Guid> timeSlotRepository)
    : IQueryHandler<Query.GetTimeSlotsWithFilterAndSortValueQuery, PagedResult<Response.TimeSlotDetailResponse>>
{
    public async Task<Result<PagedResult<Response.TimeSlotDetailResponse>>> Handle
        (Query.GetTimeSlotsWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        // Page Index and Page Size
        int PageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.TimeSlot>.DefaultPageIndex
            : request.Data.PageIndex;
        int PageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.TimeSlot>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.TimeSlot>.UpperPageSize
                ? PagedResult<Domain.Entities.TimeSlot>.UpperPageSize
                : request.Data.PageSize;

        // Handle Query SQL
        var timeSlotsQuery = new StringBuilder();

        timeSlotsQuery.Append($@"SELECT * FROM ""{nameof(Domain.Entities.TimeSlot)}""
                             WHERE ""{nameof(Domain.Entities.TimeSlot.StartTime)}""::TEXT ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = TimeSlotExtension.GetSortTimeSlotProperty(item.Key);
                timeSlotsQuery.Append($@"AND ""{nameof(Domain.Entities.TimeSlot)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    timeSlotsQuery.Append($@"'%{value}%', ");
                }

                timeSlotsQuery.Length -= 2;

                timeSlotsQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            timeSlotsQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                timeSlotsQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.TimeSlot)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.TimeSlot)}"".""{key}"" ASC, ");
            }

            timeSlotsQuery.Length -= 2;
        }

        timeSlotsQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");

        List<Domain.Entities.TimeSlot> timeSlots =
            await context.TimeSlot.FromSqlRaw(timeSlotsQuery.ToString()).ToListAsync(cancellationToken);

        int totalCount = timeSlots.Count();

        var timeSlotPagedResult =
            PagedResult<Domain.Entities.TimeSlot>.Create(timeSlots, PageIndex, PageSize, totalCount);

        PagedResult<Response.TimeSlotDetailResponse>? result =
            mapper.Map<PagedResult<Response.TimeSlotDetailResponse>>(timeSlotPagedResult);

        return Result.Success(result);
    }
}
