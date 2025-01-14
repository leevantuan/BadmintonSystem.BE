using AutoMapper;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Service;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Service;

public sealed class UpdateQuantityServiceCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IOriginalQuantityService originalQuantityService,
    IRepositoryBase<Domain.Entities.Service, Guid> serviceRepository)
    : ICommandHandler<Command.UpdateQuantityServiceCommand>
{
    public async Task<Result> Handle(Command.UpdateQuantityServiceCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Service service = await serviceRepository.FindByIdAsync(request.Data.Id, cancellationToken)
                                          ?? throw new ServiceException.ServiceNotFoundException(request.Data.Id);

        if (service.OriginalQuantityId != null)
        {
            await originalQuantityService.UpdateOriginalQuantity(service.OriginalQuantityId ?? Guid.Empty,
                request.Data.Quantity, service.QuantityPrinciple ?? 0, cancellationToken);

            return Result.Success();
        }

        service.QuantityInStock += request.Data.Quantity;

        return Result.Success();
    }
}
