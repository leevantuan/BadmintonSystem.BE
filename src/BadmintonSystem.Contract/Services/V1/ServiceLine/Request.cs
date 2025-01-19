namespace BadmintonSystem.Contract.Services.V1.ServiceLine;

public static class Request
{
    public class CreateServiceLineRequest
    {
        public Guid? ServiceId { get; set; }

        public Guid? ComboFixedId { get; set; }

        public int? Quantity { get; set; }

        public Guid? BillId { get; set; }
    }
}
