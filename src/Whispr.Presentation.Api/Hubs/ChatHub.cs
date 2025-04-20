using Microsoft.AspNetCore.SignalR;

namespace Whispr.Presentation.Api.Hubs;

public sealed class ChatHub : Hub
{
    public async Task SendMessage(string name, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", name, message);
    }
}