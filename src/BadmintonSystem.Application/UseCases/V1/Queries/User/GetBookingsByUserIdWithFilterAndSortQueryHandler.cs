using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.User;

public sealed class GetBookingsByUserIdWithFilterAndSortQueryHandler(
    IRepositoryBase<Domain.Entities.Booking, Guid> bookingRepository)
    : IQueryHandler<Query.GetBookingsByUserIdWithFilterAndSortQuery, PagedResult<Response.GetBookingByUserIdResponse>>
{
    public async Task<Result<PagedResult<Response.GetBookingByUserIdResponse>>> Handle
        (Query.GetBookingsByUserIdWithFilterAndSortQuery request, CancellationToken cancellationToken)
    {
        string yardColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Yard,
                Contract.Services.V1.Yard.Response.YardResponse>();

        string bookingColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Booking,
                Contract.Services.V1.Booking.Response.BookingDetail>();

        string timeSlotColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.TimeSlot,
                Contract.Services.V1.TimeSlot.Response.TimeSlotResponse>();

        string yardPriceColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.YardPrice,
                Contract.Services.V1.YardPrice.Response.YardPriceDetail>();

        string bookingLineColumns = StringExtension
            .TransformPropertiesToSqlAliases<BookingLine,
                Contract.Services.V1.BookingLine.Response.BookingLineResponse>();

        // Pagination
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
                                  WHERE booking.""{nameof(Domain.Entities.Booking.IsDeleted)}"" = false
                                  AND booking.""{nameof(Domain.Entities.Booking.UserId)}"" = '{request.UserId}'");

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

        var countQueryBuilder = new StringBuilder();
        countQueryBuilder.Append(
            $@"SELECT COUNT(*) AS ""{nameof(SqlResponse.TotalCountSqlResponse.TotalCount)}""");
        countQueryBuilder.Append(" \n");

        countQueryBuilder.Append(baseQueryBuilder);
        SqlResponse.TotalCountSqlResponse totalCountQueryResult = await bookingRepository
            .ExecuteSqlQuery<SqlResponse.TotalCountSqlResponse>(
                FormattableStringFactory.Create(countQueryBuilder.ToString()))
            .SingleAsync(cancellationToken);

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
            AND timeSlot.""{nameof(Domain.Entities.TimeSlot.IsDeleted)}"" = false");

        List<Contract.Services.V1.Booking.Response.GetBookingDetailSql> bookings = await bookingRepository
            .ExecuteSqlQuery<Contract.Services.V1.Booking.Response.GetBookingDetailSql>(
                FormattableStringFactory.Create(bookingQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        // Group by
        var results = bookings.GroupBy(p => p.Booking_Id)
            .Select(g => new Response.GetBookingByUserIdResponse
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
                User = g.Select(s => new Contract.Services.V1.Booking.Response.UserWithBooking
                    {
                        FullName = g.First().Booking_FullName ?? string.Empty,
                        PhoneNumber = g.First().Booking_PhoneNumber ?? string.Empty
                    })
                    .FirstOrDefault(),

                BookingLines = g.Where(x => x.BookingLine_Id != null)
                    .Select(s => new Contract.Services.V1.Booking.Response.BookingLineDetail
                    {
                        StartTime = s.TimeSlot_StartTime ?? TimeSpan.Zero,
                        EndTime = s.TimeSlot_EndTime ?? TimeSpan.Zero,
                        YardName = s.Yard_Name ?? string.Empty,
                        Price = s.BookingLine_TotalPrice ?? 0
                    })
                    .ToList()
            })
            .ToList();

        results = results.OrderBy(x => x.EffectiveDate).ToList();

        var bookingPagedResult =
            PagedResult<Response.GetBookingByUserIdResponse>.Create(
                results,
                pageIndex,
                pageSize,
                totalCountQueryResult.TotalCount);

        return Result.Success(bookingPagedResult);
    }
}
