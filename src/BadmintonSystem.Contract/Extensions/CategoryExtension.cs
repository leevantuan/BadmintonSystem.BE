namespace BadmintonSystem.Contract.Extensions;
public static class CategoryExtension
{
    public static string GetSortCategoryProperty(string sortColumn)
        => sortColumn.ToLower() switch
        {
            "name" => "Name",
            _ => "Id"
        };
}
