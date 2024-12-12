namespace BadmintonSystem.Contract.Extensions;
public static class GenderExtension
{
    public static string GetSortGenderProperty(string sortColumn)
        => sortColumn.ToLower() switch
        {
            "name" => "Name",
            _ => "Id"
        };
}
