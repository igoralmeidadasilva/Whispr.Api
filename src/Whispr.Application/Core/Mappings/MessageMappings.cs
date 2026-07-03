using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Features.Entities.Messages;

namespace Whispr.Application.Core.Mappings;

public static class MessageMappings
{
    public static MessageDto ToMessageDto(Message message)
    {
        return new MessageDto
        {
            Id = message.Id,
            SenderId = message.SenderId,
            Content = message.Content!,
            CreatedAtUtc = message.CreatedAtUtc,
            UpdatedAtUtc = message.UpdatedAtUtc
        };
    }
}