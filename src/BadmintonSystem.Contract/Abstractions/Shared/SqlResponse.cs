namespace BadmintonSystem.Contract.Abstractions.Shared;

public static class SqlResponse
{
    public class TotalCountSqlResponse
    {
        public int TotalCount { get; set; }
    }

    public class TotalPriceSqlResponse
    {
        public int TotalPrice { get; set; }
    }

    public class PriceDecimalSqlResponse
    {
        public decimal Price { get; set; }
    }
}
