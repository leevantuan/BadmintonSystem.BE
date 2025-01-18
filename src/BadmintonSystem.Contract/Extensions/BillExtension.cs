namespace BadmintonSystem.Contract.Extensions;

public static class BillExtension
{
    public static string GetSortBillProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "modifieddate" => "ModifiedDate",
            "totalprice" => "TotalPrice",
            "status" => "Status",
            _ => "Id"
        };
    }
}
