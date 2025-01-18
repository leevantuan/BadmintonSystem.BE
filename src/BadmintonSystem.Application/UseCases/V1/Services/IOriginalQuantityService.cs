namespace BadmintonSystem.Application.UseCases.V1.Services;

public interface IOriginalQuantityService
{
    Task CreateOriginalQuantity(Guid id, decimal totalQuantity, CancellationToken cancellationToken);

    Task UpdateQuantityService(Guid serviceId, decimal quantity, CancellationToken cancellationToken);
}
