namespace BadmintonSystem.Contract.Extensions;

public static class PaymentTypeExtension
{
    public static string GetSortPaymentTypeProperty(string sortColumn)
        => sortColumn.ToLower() switch
        {
            "name" => "Name",
            _ => "Id"
        };
}
