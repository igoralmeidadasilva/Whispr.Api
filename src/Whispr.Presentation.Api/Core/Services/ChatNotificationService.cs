using Microsoft.AspNetCore.SignalR;
using Whispr.Application.Core.Dtos.V1;
using Whispr.Application.Core.Services;
using Whispr.Presentation.Api.Hubs;

namespace Whispr.Presentation.Api.Core.Services;

public class ChatNotificationService : IChatNotificationService
{
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatNotificationService(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task SendAsync(MessageDto message, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.SendAsync("ReceiveMessage", message, cancellationToken);
    }
}