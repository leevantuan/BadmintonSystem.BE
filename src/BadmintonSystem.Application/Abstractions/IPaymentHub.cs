using BadmintonSystem.Contract.Services.V1.Payment;

namespace BadmintonSystem.Application.Abstractions;

public interface IPaymentHub
{
    Task SendPaymentMessageToUserAsync(string userId, Response.PaymentHubResponse message);

    Task PaymentByUserAsync(Response.PaymentHubResponse message);
}
