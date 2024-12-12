using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.PaymentMethod;

public static class Command
{
    public record CreatePaymentMethodCommand(Guid UserId, Request.CreatePaymentMethodRequest Data)
        : ICommand<Response.PaymentMethodResponse>;

    public record UpdatePaymentMethodCommand(Request.UpdatePaymentMethodRequest Data)
        : ICommand<Response.PaymentMethodResponse>;

    public record DeletePaymentMethodsCommand(List<string> Ids)
        : ICommand;
}
