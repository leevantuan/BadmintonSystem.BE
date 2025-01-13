using System.Text;
using AutoMapper;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
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
}
