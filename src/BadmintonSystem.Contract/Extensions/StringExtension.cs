using System.Text;

namespace BadmintonSystem.Contract.Extensions;

public static class StringExtension
{
    public static string Uppercase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        return input.Trim().ToUpper();
    }

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

    public static string GenerateCacheKeyFromRequest(string endPoint, DateTime date)
    {
        var onlyDate = date.Date.ToString("dd-MM-yyyy");

        var keyBuilder = new StringBuilder();
        keyBuilder.Append($"{endPoint}|{onlyDate}");

        return keyBuilder.ToString();
    }

    public static string GenerateCodeTenantFromRequest(DateTime date)
    {
        var formattedDate = date.ToString("ddMMyy");

        // Tạo chuỗi ngẫu nhiên gồm 8 ký tự (chữ cái và chữ số)
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var randomString = new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        // Kết hợp chuỗi ngày và chuỗi ngẫu nhiên
        return $"{formattedDate}_{randomString}";
    }
}
