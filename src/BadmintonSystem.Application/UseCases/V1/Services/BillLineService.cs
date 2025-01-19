using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Services;

public class BillLineService(
    ApplicationDbContext context)
    : IBillLineService
{
    public async Task OpenBillLineByBill(Guid yardId, Guid billId, CancellationToken cancellationToken)
    {
        Yard yardEntities = await context.Yard.FirstOrDefaultAsync(y => y.Id == yardId, cancellationToken)
                            ?? throw new YardException.YardNotFoundException(yardId);

        TimeSpan currentTimeSpan = DateTime.Now.TimeOfDay;

        var billLineEntities = new BillLine
        {
            Id = Guid.NewGuid(),
            BillId = billId,
            YardId = yardId,
            StartTime = currentTimeSpan,
            EndTime = currentTimeSpan,
            TotalPrice = 0,
            IsActive = ActiveEnum.IsActive
        };

        context.BillLine.Add(billLineEntities);
        await context.SaveChangesAsync(cancellationToken);

        yardEntities.IsStatus = StatusEnum.FALSE;
    }

    public async Task CloseBillLineByBill(Guid billLineId, CancellationToken cancellationToken)
    {
        BillLine billLine = await context.BillLine.FirstOrDefaultAsync(x => x.Id == billLineId, cancellationToken)
                            ?? throw new ApplicationException($"Bill line with id {billLineId} not found");

        TimeSpan currentTimeSpan = DateTime.Now.TimeOfDay;
        TimeSpan? totalTime = currentTimeSpan - billLine.StartTime;
        decimal totalHours = Math.Round((decimal)totalTime.Value.TotalMinutes / 60, 2);

        SqlResponse.PriceDecimalSqlResponse priceByYard =
            await (from yard in context.Yard
                join yardType in context.YardType on yard.YardTypeId
                    equals yardType.Id
                join price in context.Price on yardType.Id equals price
                    .YardTypeId
                where billLine.StartTime >= price.StartTime &&
                      billLine.StartTime <= price.EndTime
                select new SqlResponse.PriceDecimalSqlResponse
                {
                    Price = price.YardPrice
                }).AsNoTracking().FirstOrDefaultAsync(cancellationToken)
            ?? throw new ApplicationException("Price for yard not found");

        billLine.EndTime = currentTimeSpan;
        billLine.TotalPrice = totalHours * priceByYard.Price;
        billLine.IsActive = ActiveEnum.NotActive;

        await context.SaveChangesAsync(cancellationToken);

        Yard yardEntities = await context.Yard.FirstOrDefaultAsync(y => y.Id == billLine.YardId, cancellationToken)
                            ?? throw new YardException.YardNotFoundException(billLine.YardId ?? Guid.Empty);

        yardEntities.IsStatus = StatusEnum.TRUE;

        await context.SaveChangesAsync(cancellationToken);
    }
}
