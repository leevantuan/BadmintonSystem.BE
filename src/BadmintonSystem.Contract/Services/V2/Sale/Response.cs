namespace BadmintonSystem.Contract.Services.V2.Sale;
public static class Response
{
    public record SaleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Persent { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Status { get; set; }
    }
}
