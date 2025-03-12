namespace BadmintonSystem.Contract.Services.V1.Momo;

public static class Response
{
    public class MomoPaymentResponse
    {
        public string QrCodeUrl { get; set; } // URL QRCode thanh toán

        public string Message { get; set; } // Thông điệp từ MoMo

        public int ResultCode { get; set; } // Thay đổi kiểu dữ liệu từ string sang int

        public string PayUrl { get; set; } // Thay đổi kiểu dữ liệu từ string sang int
    }

    public class MomoPaymentResultResponse
    {
        public string PartnerCode { get; set; } = string.Empty;

        public string OrderId { get; set; } = string.Empty;

        public string RequestId { get; set; } = string.Empty;

        public int Amount { get; set; }

        public string OrderInfo { get; set; } = string.Empty;

        public string OrderType { get; set; } = string.Empty;

        public long TransId { get; set; }

        public int ResultCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public string PayType { get; set; } = string.Empty;

        public long ResponseTime { get; set; }

        public string ExtraData { get; set; } = string.Empty;

        public string Signature { get; set; } = string.Empty;
    }
}
