namespace BadmintonSystem.Application.UseCases.V1.Services;

public interface IOriginalQuantityService
{
    Task CreateOriginalQuantity(Guid id, decimal totalQuantity, CancellationToken cancellationToken);

    Task UpdateOriginalQuantity
        (Guid id, decimal quantity, decimal quantityPrinciple, CancellationToken cancellationToken);
}
