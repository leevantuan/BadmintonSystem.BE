namespace BadmintonSystem.Contract.Extensions;

public static class YardExtension
{
    public static string GetSortYardProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "name" => "Name",
            _ => "Id"
        };
    }
}
