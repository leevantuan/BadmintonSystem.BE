using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Request = BadmintonSystem.Contract.Services.V1.ServiceLine.Request;

namespace BadmintonSystem.Application.UseCases.V1.Services;

public class ServiceLineService(
    ApplicationDbContext context,
    IOriginalQuantityService originalQuantityService)
    : IServiceLineService
{
    public async Task UpdateQuantityServiceLine(Guid serviceLineId, int quantity, CancellationToken cancellationToken)
    {
        ServiceLine serviceLineEntities =
            await context.ServiceLine.FirstOrDefaultAsync(s => s.Id == serviceLineId, cancellationToken)
            ?? throw new ServiceLineException.ServiceLineNotFoundException(serviceLineId);

        int quantityChange = quantity - serviceLineEntities.Quantity;
        serviceLineEntities.Quantity = quantity;

        SqlResponse.PriceDecimalSqlResponse price =
            await GetPriceByServiceId(serviceLineEntities.ServiceId ?? Guid.Empty, cancellationToken);

        serviceLineEntities.TotalPrice = quantity * price.Price;

        await context.SaveChangesAsync(cancellationToken);

        await originalQuantityService.UpdateQuantityService(serviceLineId, quantityChange, cancellationToken);
    }

    public async Task CreateServiceLine
        (Guid billId, List<Request.CreateServiceLineRequest> serviceLines, CancellationToken cancellationToken)
    {
        _ = await context.Bill.FirstOrDefaultAsync(b => b.Id == billId, cancellationToken)
            ?? throw new BillException.BillNotFoundException(billId);

        if (serviceLines.Any())
        {
            foreach (Request.CreateServiceLineRequest serviceLine in serviceLines)
            {
                var serviceLineEntities = new ServiceLine
                {
                    Id = Guid.NewGuid(),
                    BillId = billId,
                    ServiceId = serviceLine.ServiceId,
                    Quantity = serviceLine.Quantity ?? 1
                };

                SqlResponse.PriceDecimalSqlResponse price =
                    await GetPriceByServiceId(serviceLine.ServiceId ?? Guid.Empty, cancellationToken);

                serviceLineEntities.TotalPrice =
                    CalculatorExtension.TotalPrice(price.Price, serviceLine.Quantity.Value);

                context.ServiceLine.Add(serviceLineEntities);
                await context.SaveChangesAsync(cancellationToken);

                await originalQuantityService.UpdateQuantityService(serviceLine.ServiceId.Value,
                    0 - (serviceLine.Quantity ?? 1), cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task<SqlResponse.PriceDecimalSqlResponse> GetPriceByServiceId
        (Guid serviceId, CancellationToken cancellationToken)
    {
        var query = from service in context.Service
            where service.Id == serviceId
            select new { service };

        SqlResponse.PriceDecimalSqlResponse? price = await query.AsNoTracking().Select(x =>
            new SqlResponse.PriceDecimalSqlResponse
            {
                Price = x.service.SellingPrice
            }).FirstOrDefaultAsync(cancellationToken);

        return price;
    }
}
