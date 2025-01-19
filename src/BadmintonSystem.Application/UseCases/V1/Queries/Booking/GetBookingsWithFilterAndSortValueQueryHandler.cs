using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Booking;

public sealed class GetBookingsWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context)
    : IQueryHandler<Query.GetBookingsWithFilterAndSortValueQuery, PagedResult<Response.BookingDetail>>
{
    public async Task<Result<PagedResult<Response.BookingDetail>>> Handle
        (Query.GetBookingsWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        int PageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Booking>.DefaultPageIndex
            : request.Data.PageIndex;
        int PageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Booking>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Booking>.UpperPageSize
                ? PagedResult<Domain.Entities.Booking>.UpperPageSize
                : request.Data.PageSize;

        var bookingsQuery = new StringBuilder();

        bookingsQuery.Append($@"SELECT * FROM ""{nameof(Domain.Entities.Booking)}""
                             WHERE ""{nameof(Domain.Entities.Booking.BookingTotal)}""::TEXT ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = BookingExtension.GetSortBookingProperty(item.Key);
                bookingsQuery.Append($@"AND ""{nameof(Domain.Entities.Booking)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    bookingsQuery.Append($@"'%{value}%', ");
                }

                bookingsQuery.Length -= 2;

                bookingsQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            bookingsQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                bookingsQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.Booking)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Booking)}"".""{key}"" ASC, ");
            }

            bookingsQuery.Length -= 2;
        }

        bookingsQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");

        List<Domain.Entities.Booking> bookings =
            await context.Booking.FromSqlRaw(bookingsQuery.ToString()).ToListAsync(cancellationToken);

        int totalCount = bookings.Count();

        var bookingPagedResult = PagedResult<Domain.Entities.Booking>.Create(bookings, PageIndex, PageSize, totalCount);

        PagedResult<Response.BookingDetail>? result =
            mapper.Map<PagedResult<Response.BookingDetail>>(bookingPagedResult);

        return Result.Success(result);
    }
}
