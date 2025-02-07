using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Services.V1.User;
using Microsoft.AspNetCore.SignalR;

namespace BadmintonSystem.API.Hubs;

public class RegisterHubBase : HubBase
{
}

public class RegisterHub(IHubContext<RegisterHubBase> hubContext)
    : IRegisterHub
{
    public async Task VerificationEmailAsync(Response.VerifyResponseHub message)
    {
        await hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }
}
