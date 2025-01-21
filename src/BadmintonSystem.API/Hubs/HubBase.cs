using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BadmintonSystem.API.Hubs;

[Authorize] // Tắt ai cũng thấy
public class HubBase : Hub
{
    public override async Task OnConnectedAsync()
    {
        string? userId = Context.UserIdentifier; // Get the user ID from the connection
        Console.WriteLine($"User connected: {userId}");
        await base.OnConnectedAsync();
    }
}
