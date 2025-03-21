namespace BadmintonSystem.Contract.Services.V1.Payment;

public class Request
{
    public class PaymentRequest
    {
        public string OrderId { get; set; }

        public bool Type { get; set; }
    }
}
