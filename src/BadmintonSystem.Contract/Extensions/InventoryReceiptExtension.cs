namespace BadmintonSystem.Contract.Extensions;

public class InventoryReceiptExtension
{
    public static string GetSortInventoryReceiptProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "price" => "Price",
            "quantity" => "Quantity",
            "unit" => "Unit",
            "createddate" => "CreatedDate",
            _ => "Id"
        };
    }
}
