using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.User;

public static class Command
{
    public record CreateAddressByUserIdCommand(Guid UserId, Address.Request.CreateAddressRequest Data) : ICommand;

    public record CreatePaymentMethodByUserIdCommand(
        Guid UserId,
        PaymentMethod.Request.CreatePaymentMethodRequest Data) : ICommand;

    public record ForgetPasswordCommand(Request.ForgetPasswordRequest Data) : ICommand;

    public record ChangePasswordCommand(Request.ChangePasswordRequest Data) : ICommand;
}
