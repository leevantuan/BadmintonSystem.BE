using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Services.V1.ChatMessage;
using Microsoft.AspNetCore.SignalR;

namespace BadmintonSystem.API.Hubs;

//[Authorize]
public class ChatHubBase : HubBase
{
}

public class ChatHub(IHubContext<ChatHubBase> hubContext)
    : IChatHub
{
    public async Task SendMessageToUserAsync(string userId, Response.ChatMessageResponse message)
    {
        await hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }
}
