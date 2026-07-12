using Whispr.Application.Core.Dtos.V1;
using Whispr.Domain.Features.Entities.MessageAttachments;

namespace Whispr.Application.Core.Mappings;

public static class MessageAttachmentMappings
{
    public static MessageAttachmentDto ToMessageAttachmentDto(MessageAttachment messageAttachment, string sasUri)
    {
        return new MessageAttachmentDto
        {
            Id = messageAttachment.Id,
            MessageId = messageAttachment.MessageId,
            FileName = messageAttachment.FileName,
            ContentType = messageAttachment.ContentType,
            SasUri = sasUri
        };
    }
}