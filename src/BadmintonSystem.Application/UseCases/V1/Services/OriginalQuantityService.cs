using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Services;

public class OriginalQuantityService(
    ApplicationDbContext context)
    : IOriginalQuantityService
{
    public async Task CreateOriginalQuantity(Guid id, decimal totalQuantity, CancellationToken cancellationToken)
    {
        var originalQuantities = new OriginalQuantity
        {
            Id = id,
            TotalQuantity = totalQuantity
        };

        context.OriginalQuantity.Add(originalQuantities);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateQuantityService(Guid serviceId, decimal quantity, CancellationToken cancellationToken)
    {
        Service serviceEntities = await context.Service.FirstOrDefaultAsync(x => x.Id == serviceId, cancellationToken)
                                  ?? throw new ServiceException.ServiceNotFoundException(serviceId);

        if (serviceEntities is { OriginalQuantityId: not null, QuantityPrinciple: not null })
        {
            await UpdateOriginalQuantity(serviceEntities.OriginalQuantityId.Value, quantity,
                serviceEntities.QuantityPrinciple.Value,
                cancellationToken);
        }
        else
        {
            serviceEntities.QuantityInStock += quantity;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task UpdateOriginalQuantity
        (Guid id, decimal quantity, decimal quantityPrinciple, CancellationToken cancellationToken)
    {
        OriginalQuantity? originalQuantities =
            await context.OriginalQuantity.FindAsync(new object?[] { id, cancellationToken }, cancellationToken);

        List<Service> serviceByOriginal =
            await context.Service.Where(x => x.OriginalQuantityId == originalQuantities.Id)
                .ToListAsync(cancellationToken);

        decimal? newQuantity =
            CalculatorExtension.TotalPrice(originalQuantities.TotalQuantity.Value, quantity * quantityPrinciple);

        originalQuantities.TotalQuantity = newQuantity;

        foreach (Service service in serviceByOriginal)
        {
            service.QuantityInStock =
                CalculatorExtension.QuantityInPrinciple(newQuantity.Value, service.QuantityPrinciple.Value);
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
