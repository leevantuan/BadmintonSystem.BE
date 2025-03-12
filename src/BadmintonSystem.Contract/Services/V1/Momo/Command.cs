using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Momo;

public static class Command
{
    public record CreateQRCodeCommand(Request.MomoPaymentRequest Data)
        : ICommand<Response.MomoPaymentResponse>;
}
