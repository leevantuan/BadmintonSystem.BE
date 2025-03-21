using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Payment;

public class Command
{
    public record ReturnPaymentCommand(Request.PaymentRequest Data)
    : ICommand;
}
