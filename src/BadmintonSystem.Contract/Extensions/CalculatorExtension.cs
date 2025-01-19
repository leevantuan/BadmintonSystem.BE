namespace BadmintonSystem.Contract.Extensions;

public static class CalculatorExtension
{
    public static decimal TotalPrePay(decimal price, int percent)
    {
        return price * percent / 100;
    }

    public static decimal TotalPrice(decimal price_1 = 0, decimal price_2 = 0, decimal price_3 = 0)
    {
        return price_1 + price_2 + price_3;
    }

    public static decimal TotalPrice(decimal price_1, int quantity)
    {
        return quantity * price_1;
    }

    public static decimal QuantityInPrinciple(decimal quantityInStock, decimal quantityPrinciple)
    {
        return Math.Round(quantityInStock / quantityPrinciple, 2);
    }
}
