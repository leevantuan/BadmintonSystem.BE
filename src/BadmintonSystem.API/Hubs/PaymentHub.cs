using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Services.V1.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BadmintonSystem.API.Hubs;

[Authorize]
public class PaymentHubBase : HubBase
{
}

public class PaymentHub(IHubContext<PaymentHubBase> hubContext) : IPaymentHub
{
    public async Task SendPaymentMessageToUserAsync(string userId, Response.PaymentHubResponse message)
    {
        await hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);
    }

    public async Task PaymentByUserAsync(Response.PaymentHubResponse message)
    {
        await hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }
}
