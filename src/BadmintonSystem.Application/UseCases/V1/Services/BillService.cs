using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Services;

public sealed class BillService(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Bill, Guid> billRepository)
    : IBillService
{
    public async Task DeleteBillByBookingId(Guid bookingId, CancellationToken cancellationToken)
    {
        var deleteBillQueryBuilder = new StringBuilder();
        deleteBillQueryBuilder.Append($@"DELETE FROM ""{nameof(Bill)}""
            WHERE ""{nameof(Bill.BookingId)}"" = '{bookingId}'");

        await context.Database.ExecuteSqlRawAsync(deleteBillQueryBuilder.ToString(), cancellationToken);
    }

    public async Task UpdateTotalPriceByBillId(Guid billId, CancellationToken cancellationToken)
    {
        Bill billEntities = context.Bill.FirstOrDefault(b => b.Id == billId)
                            ?? throw new BillException.BillNotFoundException(billId);

        var totalPriceQueryBuilder = new StringBuilder();
        totalPriceQueryBuilder.Append(@$"
                SELECT 
                    bill.""Id"",
                    COALESCE(SUM(billLine.""TotalPrice""), 0) AS ""BillLine_TotalPrice"",
                    COALESCE(SUM(serviceLine.""TotalPrice""),0) AS ""ServiceLine_TotalPrice"",
                    COALESCE(booking.""BookingTotal"", 0) AS ""Booking_TotalPrice"",
                    COALESCE(booking.""OriginalPrice"", 0) AS ""Booking_OriginalPrice"",
                    COALESCE(booking.""PercentPrePay"", 0) AS ""Booking_PercentPrePay""
                FROM ""Bill"" AS bill
                LEFT JOIN ""BillLine"" AS billLine 
                ON bill.""Id"" = billLine.""BillId""
                LEFT JOIN ""ServiceLine"" AS serviceLine 
                ON bill.""Id"" = serviceLine.""BillId"" 
                AND serviceLine.""IsDeleted"" = false
                LEFT JOIN ""Booking"" AS booking 
                ON bill.""BookingId"" = booking.""Id""
                WHERE bill.""Id"" = '{billId}'
                GROUP BY 
                    bill.""Id"", 
                    booking.""BookingTotal"", 
                    booking.""OriginalPrice"", 
                    booking.""PercentPrePay"" ");

        Response.GetTotalPriceSql listPrices = await billRepository.ExecuteSqlQuery<Response.GetTotalPriceSql>(
                FormattableStringFactory.Create(totalPriceQueryBuilder.ToString()))
            .FirstAsync(cancellationToken);

        decimal? totalPrice = listPrices.BillLine_TotalPrice + listPrices.ServiceLine_TotalPrice +
                              listPrices.Booking_TotalPrice;

        decimal? totalPrePay = listPrices.Booking_TotalPrice * listPrices.Booking_PercentPrePay / 100;

        billEntities.TotalPrice = totalPrice ?? 0;
        billEntities.TotalPayment = totalPrePay ?? 0;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeYardActiveByBookingId(Guid billId, StatusEnum status, CancellationToken cancellationToken)
    {
        var query = from bill in context.Bill
            join booking in context.Booking on bill.BookingId equals booking.Id
            join bookingLine in context.BookingLine on booking.Id equals bookingLine.BookingId
            join yardPrice in context.YardPrice on bookingLine.YardPriceId equals yardPrice.Id
            join yard in context.Yard on yardPrice.YardId equals yard.Id
            where bill.Id == billId
            select new { yard };

        List<Guid> ids = await query.AsNoTracking().GroupBy(x => x.yard.Id).Select(x => x.Key)
            .ToListAsync(cancellationToken);

        var yards = context.Yard.Where(x => ids.Contains(x.Id)).ToList();

        foreach (Yard yard in yards)
        {
            yard.IsStatus = status;
        }
    }
}
