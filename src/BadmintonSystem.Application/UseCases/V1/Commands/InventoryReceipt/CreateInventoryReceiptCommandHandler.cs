using AutoMapper;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.InventoryReceipt;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.InventoryReceipt;

public sealed class CreateInventoryReceiptCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IOriginalQuantityService originalQuantityService,
    IRepositoryBase<Domain.Entities.InventoryReceipt, Guid> inventoryReceiptRepository)
    : ICommandHandler<Command.CreateInventoryReceiptCommand>
{
    public async Task<Result> Handle
        (Command.CreateInventoryReceiptCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.InventoryReceipt inventoryReceipt = mapper.Map<Domain.Entities.InventoryReceipt>(request.Data);

        Domain.Entities.Service service = context.Service.FirstOrDefault(x => x.Id == request.Data.ServiceId)
                                          ?? throw new ServiceException.ServiceNotFoundException(
                                              request.Data.ServiceId ?? Guid.Empty);

        _ = context.Provider.FirstOrDefault(x => x.Id == request.Data.ProviderId)
            ?? throw new ProviderException.ProviderNotFoundException(request.Data.ProviderId);

        await originalQuantityService.UpdateQuantityService(request.Data.ServiceId ?? Guid.Empty, request.Data.Quantity,
            cancellationToken);

        inventoryReceiptRepository.Add(inventoryReceipt);

        return Result.Success();
    }
}
