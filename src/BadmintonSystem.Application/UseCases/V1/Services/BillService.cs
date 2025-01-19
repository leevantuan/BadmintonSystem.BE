using System.Runtime.CompilerServices;
using System.Text;
using BadmintonSystem.Contract.Extensions;
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
        Bill billEntities = await context.Bill.FirstOrDefaultAsync(b => b.Id == billId, cancellationToken)
                            ?? throw new BillException.BillNotFoundException(billId);

        Response.GetTotalPriceSql listPrices = await GetTotalPricesSql(billId, cancellationToken);

        billEntities.TotalPrice = CalculatorExtension.TotalPrice(listPrices.BillLine_TotalPrice,
            listPrices.ServiceLine_TotalPrice, listPrices.Booking_TotalPrice);
        billEntities.TotalPayment =
            CalculatorExtension.TotalPrePay(listPrices.Booking_TotalPrice, listPrices.Booking_PercentPrePay);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeYardActiveByBookingId(Guid billId, StatusEnum status, CancellationToken cancellationToken)
    {
        List<Yard> yards = await GetYardsByBillId(billId, cancellationToken);

        foreach (Yard yard in yards)
        {
            yard.IsStatus = status;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<List<Yard>> GetYardsByBillId(Guid billId, CancellationToken cancellationToken)
    {
        List<Guid> ids = await (from bill in context.Bill
            join booking in context.Booking on bill.BookingId equals booking.Id
            join bookingLine in context.BookingLine on booking.Id equals bookingLine.BookingId
            join yardPrice in context.YardPrice on bookingLine.YardPriceId equals yardPrice.Id
            join yard in context.Yard on yardPrice.YardId equals yard.Id
            where bill.Id == billId
            select yard.Id).Distinct().ToListAsync(cancellationToken);

        List<Yard> yards = await context.Yard.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        return yards;
    }

    private async Task<Response.GetTotalPriceSql> GetTotalPricesSql(Guid billId, CancellationToken cancellationToken)
    {
        var totalPriceQueryBuilder = new StringBuilder();
        totalPriceQueryBuilder.Append(@$"
                SELECT bill.""{nameof(Bill.Id)}"",
                    COALESCE(SUM(billLine.""{nameof(BillLine.TotalPrice)}""), 0) AS ""BillLine_TotalPrice"",
                    COALESCE(SUM(serviceLine.""{nameof(ServiceLine.TotalPrice)}""),0) AS ""ServiceLine_TotalPrice"",
                    COALESCE(booking.""{nameof(Booking.BookingTotal)}"", 0) AS ""Booking_TotalPrice"",
                    COALESCE(booking.""{nameof(Booking.OriginalPrice)}"", 0) AS ""Booking_OriginalPrice"",
                    COALESCE(booking.""{nameof(Booking.PercentPrePay)}"", 0) AS ""Booking_PercentPrePay""
                FROM ""{nameof(Bill)}"" AS bill
                LEFT JOIN ""{nameof(BillLine)}"" AS billLine 
                ON bill.""{nameof(Bill.Id)}"" = billLine.""{nameof(BillLine.BillId)}""
                LEFT JOIN ""{nameof(ServiceLine)}"" AS serviceLine 
                ON bill.""{nameof(Bill.Id)}"" = serviceLine.""{nameof(ServiceLine.BillId)}"" 
                AND serviceLine.""IsDeleted"" = false
                LEFT JOIN ""{nameof(Booking)}"" AS booking 
                ON bill.""{nameof(Bill.BookingId)}"" = booking.""{nameof(Booking.Id)}""
                WHERE bill.""{nameof(Bill.Id)}"" = '{billId}'
                GROUP BY 
                    bill.""{nameof(Bill.Id)}"", 
                    booking.""{nameof(Booking.BookingTotal)}"", 
                    booking.""{nameof(Booking.OriginalPrice)}"", 
                    booking.""{nameof(Booking.PercentPrePay)}"" ");

        Response.GetTotalPriceSql listPrices = await billRepository.ExecuteSqlQuery<Response.GetTotalPriceSql>(
                FormattableStringFactory.Create(totalPriceQueryBuilder.ToString()))
            .FirstAsync(cancellationToken);

        return listPrices;
    }
}
