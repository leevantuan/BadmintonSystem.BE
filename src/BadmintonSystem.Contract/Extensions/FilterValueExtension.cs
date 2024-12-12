namespace BadmintonSystem.Contract.Extensions;

public static class FilterValueExtension
{
    public static IDictionary<string, List<string>> ConvertStringToFilterByMultipleValueWithMultipleColumn
    (
        string? filterColumnAndMultipleValue)
    {
        var result = new Dictionary<string, List<string>>();

        if (string.IsNullOrEmpty(filterColumnAndMultipleValue))
        {
            return result;
        }

        string[] listKeyAndValueSeparateByComma = filterColumnAndMultipleValue.Trim().Split(',');

        foreach (string item in listKeyAndValueSeparateByComma)
        {
            if (!item.Contains('-'))
            {
                throw new FormatException(
                    "Filter condition should be followed by format: Column1-Value, Column2-Value...");
            }

            string[] keyAndValue = item.ToLower().Trim().Split('-');

            // case value is Guid type
            if (keyAndValue.Length != 2)
            {
                throw new FormatException(
                    "Each filter condition must have exactly one '-' to separate column and value.");
            }

            string key = keyAndValue[0].Trim();
            string value = keyAndValue[1].Trim();

            if (!result.ContainsKey(key))
            {
                result[key] = new List<string>();
            }

            if (!result[key].Contains(value))
            {
                result[key].Add(value); // Add value if not already present
            }
        }

        return result;
    }
}
