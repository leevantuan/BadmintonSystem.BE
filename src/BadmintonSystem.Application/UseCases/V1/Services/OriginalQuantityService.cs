using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence;

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

    public async Task UpdateOriginalQuantity
        (Guid id, decimal quantity, decimal quantityPrinciple, CancellationToken cancellationToken)
    {
        OriginalQuantity? originalQuantities =
            await context.OriginalQuantity.FindAsync(new object?[] { id, cancellationToken }, cancellationToken);

        var serviceByOriginal =
            context.Service.Where(x => x.OriginalQuantityId == originalQuantities.Id).ToList();

        decimal? newQuantity = originalQuantities.TotalQuantity + quantity * quantityPrinciple;

        originalQuantities.TotalQuantity = newQuantity;

        foreach (Service service in serviceByOriginal)
        {
            decimal newQuantityService = (decimal)(newQuantity / service.QuantityPrinciple);
            service.QuantityInStock = Math.Round(newQuantityService, 2);
        }
    }
}
