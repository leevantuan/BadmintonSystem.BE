using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Service;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Service;

public sealed class UpdateQuantityServiceCommandHandler(
    IOriginalQuantityService originalQuantityService,
    IRepositoryBase<Domain.Entities.Service, Guid> serviceRepository)
    : ICommandHandler<Command.UpdateQuantityServiceCommand>
{
    public async Task<Result> Handle(Command.UpdateQuantityServiceCommand request, CancellationToken cancellationToken)
    {
        _ = await serviceRepository.FindByIdAsync(request.Data.Id, cancellationToken)
            ?? throw new ServiceException.ServiceNotFoundException(request.Data.Id);

        await originalQuantityService.UpdateQuantityService(request.Data.Id, request.Data.Quantity, cancellationToken);

        return Result.Success();
    }
}
