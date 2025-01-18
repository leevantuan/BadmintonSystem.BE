using AutoMapper;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.InventoryReceipt;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.InventoryReceipt;

public sealed class DeleteInventoryReceiptsCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IOriginalQuantityService originalQuantityService,
    IRepositoryBase<Domain.Entities.InventoryReceipt, Guid> inventoryReceiptRepository)
    : ICommandHandler<Command.DeleteInventoryReceiptsCommand>
{
    public async Task<Result> Handle
        (Command.DeleteInventoryReceiptsCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.InventoryReceipt inventoryReceipt =
            context.InventoryReceipt.FirstOrDefault(x => x.Id == request.Id)
            ?? throw new InventoryReceiptException.InventoryReceiptNotFoundException(request.Id);

        _ = context.Service.FirstOrDefault(x => x.Id == inventoryReceipt.ServiceId)
            ?? throw new ServiceException.ServiceNotFoundException(
                inventoryReceipt.ServiceId ?? Guid.Empty);

        await originalQuantityService.UpdateQuantityService(inventoryReceipt.ServiceId ?? Guid.Empty,
            (decimal)(0 - inventoryReceipt.Quantity), cancellationToken);

        inventoryReceiptRepository.Remove(inventoryReceipt);

        return Result.Success();
    }
}
