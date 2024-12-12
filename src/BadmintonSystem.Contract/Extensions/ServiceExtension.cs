namespace BadmintonSystem.Contract.Extensions;
public static class ServiceExtension
{
    public static string GetSortServiceProperty(string sortColumn)
        => sortColumn.ToLower() switch
        {
            "name" => "Name",
            "sellingprice" => "SellingPrice",
            "purchaseprice" => "PurchasePrice",
            _ => "Id"
        };
}
