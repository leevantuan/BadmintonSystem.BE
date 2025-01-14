namespace BadmintonSystem.Contract.Services.V1.OriginalQuantity;

public static class Request
{
    public class CreateOriginalQuantityRequest
    {
        public decimal? TotalQuantity { get; set; }
    }

    public class UpdateOriginalQuantityRequest
    {
        public Guid? Id { get; set; }

        public decimal? TotalQuantity { get; set; }
    }
}
