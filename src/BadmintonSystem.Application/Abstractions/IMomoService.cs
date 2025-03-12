using BadmintonSystem.Contract.Services.V1.Momo;

namespace BadmintonSystem.Application.Abstractions;

public interface IMomoService
{
    Task<string> CreatePaymentQRCodeAsync(Request.MomoPaymentRequest request, CancellationToken cancellationToken);
}
