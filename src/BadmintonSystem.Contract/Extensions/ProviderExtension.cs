namespace BadmintonSystem.Contract.Extensions;

public static class ProviderExtension
{
    public static string GetSortProviderProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "name" => "Name",
            "address" => "Address",
            "phonenumber" => "PhoneNumber",
            _ => "Id"
        };
    }
}
