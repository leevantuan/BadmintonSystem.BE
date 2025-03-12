namespace BadmintonSystem.Contract.Services.V1.Momo;

public static class Request
{
    public class MomoPaymentRequest
    {
        public string OrderId { get; set; } // Mã đơn hàng

        public string Amount { get; set; } // Số tiền thanh toán

        public string OrderInfo { get; set; } // Thông tin đơn hàng
    }
}
