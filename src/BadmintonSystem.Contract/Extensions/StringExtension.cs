namespace BadmintonSystem.Contract.Extensions;

public static class StringExtension
{
    public static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }

    public static string ConvertEnumNameToSpacedString(string input)
    {
        // convert CreditCard => Credit Card
        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x : x.ToString()));
    }

    public static string GetFullNameFromFirstNameAndLastName(string firstName, string lastName)
    {
        string fullName = $"{lastName.Trim()} {firstName.Trim()}";

        return fullName.Trim();
    }

    public static string TransformPropertiesToSqlAliases<TEntity, TResponse>()
        where TEntity : class
        where TResponse : class
    {
        string entityNameToLower = typeof(TEntity).Name.ToLower();

        string columns = string.Join(
            ", ",
            typeof(TResponse)
                .GetProperties()
                .Where(p => !p.PropertyType.IsGenericType ||
                            !p.PropertyType.IsClass) // Exclude collections like Products
                .Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string))
                .Select(p => $@"{entityNameToLower}.""{p.Name}"" AS ""{typeof(TEntity).Name}_{p.Name}"""));

        return columns;
    }
}
