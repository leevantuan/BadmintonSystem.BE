using System.Text;
using System.Text.Json;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Services.V1.Momo;
using BadmintonSystem.Infrastructure.DependencyInjection.Options;
using Microsoft.Extensions.Configuration;

namespace BadmintonSystem.Infrastructure.Services;

public class MomoService : IMomoService
{
    private readonly MomoOption _momoOption = new();

    public MomoService(IConfiguration configuration)
    {
        configuration.GetSection(nameof(MomoOption)).Bind(_momoOption);
    }

    public async Task<string> CreatePaymentQRCodeAsync
        (Request.MomoPaymentRequest request, CancellationToken cancellationToken)
    {
        using var client = new HttpClient();

        var endpoint = _momoOption.PaymentUrl;
        var requestId = Guid.NewGuid().ToString();
        var orderInfo = string.IsNullOrEmpty(request.OrderInfo)
            ? $"Thanh toán đơn hàng: {request.OrderId}"
            : request.OrderInfo;

        // Chuẩn bị payload
        var payload = new
        {
            partnerCode = _momoOption.PartnerCode,
            accessKey = _momoOption.AccessKey,
            requestId = requestId ?? Guid.NewGuid().ToString(),
            amount = request.Amount,
            orderId = request.OrderId,
            orderInfo = orderInfo ?? string.Empty,
            redirectUrl = _momoOption.ReturnUrl,
            ipnUrl = _momoOption.IpnUrl,
            extraData = string.Empty,
            requestType = "captureWallet",
            lang = "vi",
            signature = GenerateSignature(request, requestId, orderInfo)
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        try
        {
            // Gửi request tới MoMo
            var response = await client.PostAsync(endpoint, content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
            var jsonResponse = JsonSerializer.Deserialize<Response.MomoPaymentResponse>(
                responseString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Trả về link QRCode từ response
            return jsonResponse?.PayUrl;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Lỗi khi gọi API MoMo: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception($"Lỗi khi xử lý phản hồi từ MoMo: {ex.Message}", ex);
        }
    }

    private string GenerateSignature(Request.MomoPaymentRequest request, string requestId, string orderInfo)
    {
        // Tạo chuỗi dữ liệu để ký theo đúng quy định của MoMo
        string rawData =
            $"accessKey={_momoOption.AccessKey}" +
            $"&amount={request.Amount}" +
            $"&extraData=" +
            $"&ipnUrl={_momoOption.IpnUrl}" +
            $"&orderId={request.OrderId}" +
            $"&orderInfo={orderInfo}" +
            $"&partnerCode={_momoOption.PartnerCode}" +
            $"&redirectUrl={_momoOption.ReturnUrl}" +
            $"&requestId={requestId}" +
            $"&requestType=captureWallet";

        return HmacSHA256(rawData, _momoOption.SecretKey);
    }

    private string HmacSHA256(string data, string key)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
