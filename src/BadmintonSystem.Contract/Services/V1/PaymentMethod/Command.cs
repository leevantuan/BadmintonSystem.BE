using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.PaymentMethod;

public static class Command
{
    public record CreatePaymentMethodByUserIdCommand(
        Guid UserId,
        Request.CreatePaymentMethodRequest Data) : ICommand;

    public record UpdatePaymentMethodByUserIdCommand(
        Guid UserId,
        Request.UpdatePaymentMethodRequest Data) : ICommand;

    public record DeletePaymentMethodByUserIdCommand(
        Guid UserId,
        Guid PaymentMethodId) : ICommand;
}
