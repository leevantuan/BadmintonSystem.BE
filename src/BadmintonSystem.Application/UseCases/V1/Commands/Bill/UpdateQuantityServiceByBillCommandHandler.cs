using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Bill;

public sealed class UpdateQuantityServiceByBillCommandHandler(
    IServiceLineService serviceLineService,
    IRepositoryBase<ServiceLine, Guid> serviceLineRepository)
    : ICommandHandler<Command.UpdateQuantityServiceByBillCommand>
{
    public async Task<Result> Handle
        (Command.UpdateQuantityServiceByBillCommand request, CancellationToken cancellationToken)
    {
        _ = await serviceLineRepository.FindByIdAsync(request.Data.ServiceLineId, cancellationToken)
            ?? throw new ServiceLineException.ServiceLineNotFoundException(request.Data.ServiceLineId);

        await serviceLineService.UpdateQuantityServiceLine(request.Data.ServiceLineId, (int)request.Data.Quantity,
            cancellationToken);

        return Result.Success();
    }
}
