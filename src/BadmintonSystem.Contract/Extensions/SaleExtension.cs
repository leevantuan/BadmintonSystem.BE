namespace BadmintonSystem.Contract.Extensions;

public static class SaleExtension
{
    public static string GetSortSaleProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "name" => "Name",
            "percent" => "Percent",
            "startdate" => "StartDate",
            "enddate" => "EndDate",
            _ => "Id"
        };
    }
}
