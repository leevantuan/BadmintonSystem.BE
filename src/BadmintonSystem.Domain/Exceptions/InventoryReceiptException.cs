namespace BadmintonSystem.Domain.Exceptions;

public class InventoryReceiptException
{
    public class InventoryReceiptNotFoundException : NotFoundException
    {
        public InventoryReceiptNotFoundException(Guid inventoryReceiptId)
            : base($"The Inventory Receipt with the id {inventoryReceiptId} was not found.")
        {
        }
    }
}
