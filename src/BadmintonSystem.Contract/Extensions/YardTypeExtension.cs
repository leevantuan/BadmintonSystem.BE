namespace BadmintonSystem.Contract.Extensions;

public static class YardTypeExtension
{
    public static string GetSortYardTypeProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "name" => "Name",
            "price" => "Price",
            _ => "Id"
        };
    }
}
