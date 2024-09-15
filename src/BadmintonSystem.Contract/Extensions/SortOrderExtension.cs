using BadmintonSystem.Contract.Enumerations;

namespace BadmintonSystem.Contract.Extensions;
public static class SortOrderExtension
{
    public static SortOrder ConvertStringToSortOrder(string? sortOrder)
        => !string.IsNullOrWhiteSpace(sortOrder)
           ? sortOrder.Trim().ToLower().Equals("asc") // If contain Asc == SortOrder.Ascending
           ? SortOrder.Ascending : SortOrder.Descending : SortOrder.Ascending;

    // Format: Column1-ASC,Column2-DESC...
    public static IDictionary<string, SortOrder> ConvertStringToSortOrderV2(string? sortOrder)
    {
        var result = new Dictionary<string, SortOrder>();

        if (!string.IsNullOrEmpty(sortOrder))
        {
            if (sortOrder.Trim().Split(",").Length > 0)
            {
                foreach (var item in sortOrder.Split(","))
                {
                    if (!item.Contains('-'))
                        throw new FormatException("Sort condition should be follow by format: Column1-ASC,Column2-DESC...");
                    var property = item.Trim().Split("-"); // Remove Space and split -
                    var key = GenderExtension.GetSortGenderProperty(property[0]); // This is KEY
                    var value = ConvertStringToSortOrder(property[1]); // This is VALUE
                    result.TryAdd(key, value); // Remove Duplicate
                }
            }
            else
            {
                if (!sortOrder.Contains('-'))
                    throw new FormatException("Sort condition should be follow by format: Column1-ASC,Column2-DESC...");

                var property = sortOrder.Trim().Split("-");
                result.Add(property[0], ConvertStringToSortOrder(property[1]));
            }
        }

        return result;
    }
}
