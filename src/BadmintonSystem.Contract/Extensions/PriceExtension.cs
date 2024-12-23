namespace BadmintonSystem.Contract.Extensions;

public static class PriceExtension
{
    public static string GetSortPriceProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "yardprice" => "YardPrice",
            "isdefault" => "IsDefault",
            _ => "Id"
        };
    }
}
