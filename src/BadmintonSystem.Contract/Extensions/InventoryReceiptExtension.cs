namespace BadmintonSystem.Contract.Extensions;

public class InventoryReceiptExtension
{
    public static string GetSortInventoryReceiptProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "price" => "Price",
            "createddate" => "CreatedDate",
            _ => "Id"
        };
    }
}
