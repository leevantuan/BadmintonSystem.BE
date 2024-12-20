using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.User;

public static class Command
{
    // Review
    // public record CreateReviewByUserIdCommand(Guid UserId, Request.CreateReviewByUserIdRequest Data) : ICommand;
    //
    // public record UpdateReviewByUserIdCommand(Guid UserId, Request.UpdateReviewByUserIdRequest Data) : ICommand;
    //
    // public record DeleteReviewByUserIdCommand(Guid UserId, Guid ReviewId) : ICommand;

    // Address
    // public record CreateAddressByUserIdCommand(Guid UserId, Address.Request.CreateAddressRequest Data) : ICommand;
    //
    // public record UpdateAddressByUserIdCommand(Guid UserId, Request.UpdateAddressByUserIdRequest Data) : ICommand;
    //
    // public record DeleteAddressByUserIdCommand(Guid UserId, Guid AddressId) : ICommand;

    // Payment method
    // public record CreatePaymentMethodByUserIdCommand(
    //     Guid UserId,
    //     PaymentMethod.Request.CreatePaymentMethodRequest Data) : ICommand;
    //
    // public record UpdatePaymentMethodByUserIdCommand(
    //     Guid UserId,
    //     PaymentMethod.Request.UpdatePaymentMethodRequest Data) : ICommand;
    //
    // public record DeletePaymentMethodByUserIdCommand(
    //     Guid UserId,
    //     Guid PaymentMethodId) : ICommand;

    // Notification
    // public record DeleteNotificationByUserIdCommand(
    //     Guid UserId,
    //     Guid NotificationId) : ICommand;

    public record ForgetPasswordCommand(Request.ForgetPasswordRequest Data) : ICommand;

    public record ChangePasswordCommand(Request.ChangePasswordRequest Data) : ICommand;
}
