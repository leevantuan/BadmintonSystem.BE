namespace BadmintonSystem.Contract.Extensions;
public static class PaymentMethodExtension
{
    public static string GetSortPaymentMethodProperty(string sortColumn)
        => sortColumn.ToLower() switch
        {
            "provider" => "Provider",
            "accountnumber" => "AccountNumber",
            "expiry" => "Expiry",
            "isdefault" => "IsDefault",
            _ => "Id"
        };
}
