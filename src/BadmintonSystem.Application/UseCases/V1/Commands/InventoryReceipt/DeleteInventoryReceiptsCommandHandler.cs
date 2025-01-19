using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.InventoryReceipt;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.InventoryReceipt;

public sealed class DeleteInventoryReceiptsCommandHandler(
    ApplicationDbContext context,
    IOriginalQuantityService originalQuantityService,
    IRepositoryBase<Domain.Entities.InventoryReceipt, Guid> inventoryReceiptRepository)
    : ICommandHandler<Command.DeleteInventoryReceiptsCommand>
{
    public async Task<Result> Handle
        (Command.DeleteInventoryReceiptsCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.InventoryReceipt inventoryReceipt =
            await context.InventoryReceipt.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new InventoryReceiptException.InventoryReceiptNotFoundException(request.Id);

        _ = await context.Service.FirstOrDefaultAsync(x => x.Id == inventoryReceipt.ServiceId, cancellationToken)
            ?? throw new ServiceException.ServiceNotFoundException(
                inventoryReceipt.ServiceId ?? Guid.Empty);

        await originalQuantityService.UpdateQuantityService(inventoryReceipt.ServiceId.Value,
            (decimal)(0 - inventoryReceipt.Quantity), cancellationToken);

        inventoryReceiptRepository.Remove(inventoryReceipt);

        return Result.Success();
    }
}
