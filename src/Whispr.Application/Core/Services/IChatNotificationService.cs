using Whispr.Application.Core.Dtos.V1;

namespace Whispr.Application.Core.Services;

public interface IChatNotificationService
{
    Task SendAsync(MessageDto message, CancellationToken cancellationToken);
}