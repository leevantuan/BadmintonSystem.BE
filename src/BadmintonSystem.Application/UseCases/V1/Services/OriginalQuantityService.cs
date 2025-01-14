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
}
