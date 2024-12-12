namespace BadmintonSystem.Contract.Services.V1.Sale;

public static class Request
{
    public record CreateSaleRequest(string Name, int Percent, DateTime StartDate, DateTime EndDate, int IsActive);

    public class UpdateSaleRequest
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public int? Percent { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int IsActive { get; set; }
    }
}
