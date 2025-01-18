using BadmintonSystem.Contract.Services.V1.ServiceLine;

namespace BadmintonSystem.Application.UseCases.V1.Services;

public interface IServiceLineService
{
    Task UpdateQuantityServiceLine(Guid serviceLineId, int quantity, CancellationToken cancellationToken);

    Task CreateServiceLine
        (Guid billId, List<Request.CreateServiceLineRequest> serviceLines, CancellationToken cancellationToken);
}
