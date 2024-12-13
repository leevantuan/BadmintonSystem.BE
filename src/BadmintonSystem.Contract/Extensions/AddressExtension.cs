namespace BadmintonSystem.Contract.Extensions;

public static class AddressExtension
{
    public static string GetSortAddressProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "unit" => "Unit",
            "street" => "Street",
            "addressline1" => "AddressLine1",
            "addressline2" => "AddressLine2",
            "city" => "City",
            "province" => "Province",
            _ => "Id"
        };
    }
}
