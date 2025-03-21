namespace BadmintonSystem.Contract.Services.V1.Payment;

public class Response
{
    public class PaymentHubResponse
    {
        public string OrderId { get; set; }

        public bool Type { get; set; }
    }
}
