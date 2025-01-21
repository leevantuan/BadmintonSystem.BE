namespace BadmintonSystem.Contract.Extensions;

public static class AppUserExtension
{
    public static string GetSortAppUserProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "firstName" => "FirstName",
            "lastname" => "LastName",
            "fullname" => "FullName",
            "username" => "UserName",
            _ => "CreatedDate"
        };
    }
}
