namespace BadmintonSystem.Contract.Extensions;
public static class UserAddressExtension
{
    public static string GetSortUserAddressProperty(string sortColumn)
        => sortColumn.ToLower() switch
        {
            "isdefault" => "IsDefault",
            _ => "Id"
        };
}
