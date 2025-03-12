using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Momo;
using Response = BadmintonSystem.Contract.Services.V1.Momo.Response;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Momo;

public sealed class CreateQRCodeCommandHandler(
    IMomoService momoService)
    : ICommandHandler<Command.CreateQRCodeCommand, Response.MomoPaymentResponse>
{
    public async Task<Result<Response.MomoPaymentResponse>> Handle
        (Command.CreateQRCodeCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Request cannot be null.");
        }

        try
        {
            string qrCodeUrl = await momoService.CreatePaymentQRCodeAsync(request.Data, cancellationToken);

            var response = new Response.MomoPaymentResponse
            {
                ResultCode = 0,
                Message = "QR Code generated successfully.",
                QrCodeUrl = "QRCode",
                PayUrl = qrCodeUrl
            };
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to create QR Code", ex);
        }
    }
}
