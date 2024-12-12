namespace BadmintonSystem.Contract.Extensions;
public static class ClubAddressExtension
{
    public static string GetSortClubAddressProperty(string sortColumn)
        => sortColumn.ToLower() switch
        {
            "branch" => "Branch",
            _ => "Id"
        };
}
