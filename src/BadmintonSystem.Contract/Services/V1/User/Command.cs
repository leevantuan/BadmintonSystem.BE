using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.User;

public static class Command
{
    public record CreateAddressByUserIdCommand(Guid UserId, Address.Request.CreateAddressRequest Data) : ICommand;

    public record UpdateAddressByUserIdCommand(Guid UserId, Request.UpdateAddressByUserIdRequest Data) : ICommand;

    public record DeleteAddressByUserIdCommand(Guid UserId, Guid AddressId) : ICommand;

    public record CreatePaymentMethodByUserIdCommand(
        Guid UserId,
        PaymentMethod.Request.CreatePaymentMethodRequest Data) : ICommand;

    public record UpdatePaymentMethodByUserIdCommand(
        Guid UserId,
        PaymentMethod.Request.UpdatePaymentMethodRequest Data) : ICommand;

    public record DeletePaymentMethodByUserIdCommand(
        Guid UserId,
        Guid PaymentMethodId) : ICommand;

    public record DeleteNotificationByUserIdCommand(
        Guid UserId,
        Guid NotificationId) : ICommand;

    public record ForgetPasswordCommand(Request.ForgetPasswordRequest Data) : ICommand;

    public record ChangePasswordCommand(Request.ChangePasswordRequest Data) : ICommand;
}
