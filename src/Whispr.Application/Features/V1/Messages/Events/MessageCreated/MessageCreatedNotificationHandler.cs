using Whispr.Application.Core.Dtos.V1;
using Whispr.Application.Core.Services;

namespace Whispr.Application.Features.V1.Messages.Events.MessageCreated;

internal sealed class MessageCreatedNotificationHandler : INotificationHandler<MessageCreatedNotification>
{
    private readonly IChatNotificationService _chatNotificationService;

    public MessageCreatedNotificationHandler(IChatNotificationService chatNotificationService)
    {
        _chatNotificationService = chatNotificationService;
    }

    public async Task Handle(MessageCreatedNotification notification, CancellationToken cancellationToken)
    {
        MessageDto messageDto = new()
        {
            Id = notification.Id,
            UserId = notification.UserId,
            User =  new UserDto
            {
                Id =  notification.UserId,
                Name = notification.UserName
            },
            Content = notification.Content,
            CreatedAtUtc = notification.CreatedAtUtc,
            UpdatedAtUtc = notification.UpdatedAtUtc,
            Attachments = notification.Attachments
        };

        await _chatNotificationService.SendAsync(messageDto, cancellationToken);
    }
}