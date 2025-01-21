using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Services.V1.Bill;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BadmintonSystem.API.Hubs;

[Authorize]
public class BookingHubBase : HubBase
{
}

public class BookingHub(IHubContext<BookingHubBase> hubContext)
    : IBookingHub
{
    public async Task SendBookingMessageToUserAsync(string userId, Response.BookingHubResponse message)
    {
        await hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);
    }

    public async Task BookingByUserAsync(Response.BookingHubResponse message)
    {
        await hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }
}
