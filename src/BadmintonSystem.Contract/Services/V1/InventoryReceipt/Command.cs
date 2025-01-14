using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.InventoryReceipt;

public static class Command
{
    public record CreateInventoryReceiptCommand(Guid UserId, Request.CreateInventoryReceiptRequest Data)
        : ICommand;

    public record DeleteInventoryReceiptsCommand(Guid Id)
        : ICommand;
}
