using System.Runtime.CompilerServices;
using System.Text;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Booking;

public sealed class GetBookingsByDateFilterAndSortValueQueryHandler(
    IRepositoryBase<Domain.Entities.Booking, Guid> bookingRepository)
    : IQueryHandler<Query.GetBookingsByDateFilterAndSortValueQuery, PagedResult<Response.GetBookingDetailResponse>>
{
    public async Task<Result<PagedResult<Response.GetBookingDetailResponse>>> Handle
        (Query.GetBookingsByDateFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        string yardColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Yard,
                Contract.Services.V1.Yard.Response.YardResponse>();

        string bookingColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Booking,
                Response.BookingDetail>();

        string timeSlotColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.TimeSlot,
                Contract.Services.V1.TimeSlot.Response.TimeSlotResponse>();

        string yardPriceColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.YardPrice,
                Contract.Services.V1.YardPrice.Response.YardPriceDetail>();

        string bookingLineColumns = StringExtension
            .TransformPropertiesToSqlAliases<BookingLine,
                Contract.Services.V1.BookingLine.Response.BookingLineResponse>();

        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Address>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Address>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Address>.UpperPageSize
                ? PagedResult<Domain.Entities.Address>.UpperPageSize
                : request.Data.PageSize;

        var bookingQueryBuilder = new StringBuilder();

        bookingQueryBuilder.Append(@"WITH bookingTemp AS (
                                    SELECT *");
        bookingQueryBuilder.Append(" \n");

        var baseQueryBuilder = new StringBuilder();

        baseQueryBuilder.Append($@"FROM ""{nameof(Domain.Entities.Booking)}"" AS booking
                                  WHERE booking.""{nameof(Domain.Entities.Booking.IsDeleted)}"" = false");

        baseQueryBuilder.Append(" \n");

        // FILTER MULTIPLE
        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = BookingExtension.GetSortBookingProperty(item.Key);
                baseQueryBuilder.Append($@"AND booking.""{key}""::TEXT ILIKE ANY (ARRAY[");

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
                    ? $@" booking.""{key}"" DESC, "
                    : $@" booking.""{key}"" ASC, ");
            }

            baseQueryBuilder.Length -= 2;
        }

        int totalCount = await TotalCount(baseQueryBuilder.ToString(), cancellationToken);

        bookingQueryBuilder.Append(baseQueryBuilder);
        bookingQueryBuilder.Append($@"OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");
        bookingQueryBuilder.Append(" \n )");
        bookingQueryBuilder.Append(" \n");
        bookingQueryBuilder.Append(
            $@"SELECT {bookingColumns}, {yardColumns}, {bookingLineColumns}, {timeSlotColumns}, {yardPriceColumns}
            FROM bookingTemp AS booking
            JOIN ""{nameof(BookingLine)}"" AS bookingLine
            ON bookingLine.""{nameof(BookingLine.BookingId)}"" = booking.""{nameof(Domain.Entities.Booking.Id)}""
            AND bookingLine.""{nameof(BookingLine.IsDeleted)}"" = false
            JOIN ""{nameof(Domain.Entities.YardPrice)}"" AS yardPrice
            ON yardPrice.""{nameof(Domain.Entities.YardPrice.Id)}"" = bookingLine.""{nameof(BookingLine.YardPriceId)}""
            AND yardPrice.""{nameof(Domain.Entities.YardPrice.IsDeleted)}"" = false
            JOIN ""{nameof(Domain.Entities.Yard)}"" AS yard
            ON yard.""{nameof(Domain.Entities.Yard.Id)}"" = yardPrice.""{nameof(Domain.Entities.YardPrice.YardId)}""
            AND yard.""{nameof(Domain.Entities.Yard.IsDeleted)}"" = false
            JOIN ""{nameof(Domain.Entities.TimeSlot)}"" AS timeSlot
            ON timeSlot.""{nameof(Domain.Entities.TimeSlot.Id)}"" = yardPrice.""{nameof(Domain.Entities.YardPrice.TimeSlotId)}""
            AND timeSlot.""{nameof(Domain.Entities.TimeSlot.IsDeleted)}"" = false
            WHERE yardPrice.""{nameof(Domain.Entities.YardPrice.EffectiveDate)}""::DATE = '{request.Date.Date}'");

        List<Response.GetBookingDetailSql> bookings = await bookingRepository
            .ExecuteSqlQuery<Response.GetBookingDetailSql>(
                FormattableStringFactory.Create(bookingQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        var bookingPagedResult =
            PagedResult<Response.GetBookingDetailResponse>.Create(
                GroupByData(bookings),
                pageIndex,
                pageSize,
                totalCount);

        return Result.Success(bookingPagedResult);
    }

    private async Task<int> TotalCount(string baseQuery, CancellationToken cancellationToken)
    {
        var countQueryBuilder = new StringBuilder();
        countQueryBuilder.Append(
            $@"SELECT COUNT(*) AS ""{nameof(SqlResponse.TotalCountSqlResponse.TotalCount)}""");
        countQueryBuilder.Append(" \n");

        countQueryBuilder.Append(baseQuery);
        SqlResponse.TotalCountSqlResponse totalCountQueryResult = await bookingRepository
            .ExecuteSqlQuery<SqlResponse.TotalCountSqlResponse>(
                FormattableStringFactory.Create(countQueryBuilder.ToString()))
            .SingleAsync(cancellationToken);

        return totalCountQueryResult.TotalCount;
    }

    private List<Response.GetBookingDetailResponse> GroupByData(List<Response.GetBookingDetailSql> bookings)
    {
        var results = bookings.GroupBy(p => p.Booking_Id)
            .Select(g => new Response.GetBookingDetailResponse
            {
                Id = g.Key ?? Guid.Empty,
                BookingDate = g.First().Booking_BookingDate ?? DateTime.Now,
                EffectiveDate = g.First().YardPrice_EffectiveDate ?? DateTime.Now,
                BookingTotal = g.First().Booking_BookingTotal ?? 0,
                OriginalPrice = g.First().Booking_OriginalPrice ?? 0,
                UserId = g.First().Booking_UserId ?? Guid.Empty,
                SaleId = g.First().Booking_SaleId ?? Guid.Empty,
                BookingStatus = g.First().Booking_BookingStatus ?? 0,
                PaymentStatus = g.First().Booking_PaymentStatus ?? 0,
                FullName = g.First().Booking_FullName ?? string.Empty,
                PhoneNumber = g.First().Booking_PhoneNumber ?? string.Empty,
                User = g.Select(s => new Response.UserWithBooking
                    {
                        FullName = g.First().Booking_FullName ?? string.Empty,
                        PhoneNumber = g.First().Booking_PhoneNumber ?? string.Empty
                    })
                    .FirstOrDefault(),

                BookingLines = g.Where(x => x.BookingLine_Id != null)
                    .Select(s => new Response.BookingLineDetail
                    {
                        StartTime = s.TimeSlot_StartTime ?? TimeSpan.Zero,
                        EndTime = s.TimeSlot_EndTime ?? TimeSpan.Zero,
                        YardName = s.Yard_Name ?? string.Empty,
                        Price = s.BookingLine_TotalPrice ?? 0
                    })
                    .ToList()
            })
            .ToList();

        return results;
    }
}
