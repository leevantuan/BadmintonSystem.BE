namespace BadmintonSystem.Contract.Extensions;
public class GenderExtension
{
    public static string GetSortGenderProperty(string sortColumn)
       => sortColumn.ToLower() switch
       {
           "name" => "Name",
           //"price" => "Price", // Example fields
           //"description" => "Description",
           _ => "Id"
       };
}
