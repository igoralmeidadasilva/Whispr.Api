using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Whispr.Application.Core.Options;
using Whispr.Application.Core.Services;
using Whispr.Application.Features.V1.Messages.Events.MessageCreated;
using Whispr.Domain.Features.Entities.MessageAttachments;
using Whispr.Domain.Features.Entities.Messages;
using Whispr.Domain.Features.Entities.Users;

namespace Whispr.Application.Features.V1.Messages.Commands.Create;

internal sealed class CreateMessageCommandHandler : ICommandHandler<CreateMessageCommand, Unit>
{
    private readonly IMessagePersistenceRepository _messagePersistenceRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;
    private readonly IStorageService _storageService;
    private readonly StorageOptions _storageOptions;
    private readonly ILogger<CreateMessageCommandHandler> _logger;

    public CreateMessageCommandHandler(
        IMessagePersistenceRepository messagePersistenceRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork,
        IPublisher publisher,
        IStorageService storageService,
        IOptions<StorageOptions> storageOptions,
        ILogger<CreateMessageCommandHandler> logger)
    {
        _messagePersistenceRepository = messagePersistenceRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
        _storageService = storageService;
        _storageOptions = storageOptions.Value;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userReadOnlyRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result<Unit>.Failure(CreateMessageCommandErrors.UserNotFound);
        }

        Message newMessage = new(request.UserId, request.Content);

        if (request.Attachments is not null)
        {
            await AttachFilesToMessage(newMessage, request.Attachments, cancellationToken);
        }
        
        _messagePersistenceRepository.Insert(newMessage);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    
        await _publisher.Publish(new MessageCreatedNotification
        {
            Id = newMessage.Id,
            UserId = newMessage.UserId,
            UserName = user!.Name!,
            Content = newMessage.Content,
            CreatedAtUtc = newMessage.CreatedAtUtc,
            UpdatedAtUtc = newMessage.UpdatedAtUtc
        }, cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }

    private async Task AttachFilesToMessage(
        Message message,
        IEnumerable<IFormFile> attachments,
        CancellationToken cancellationToken = default)
    {
        foreach (IFormFile attachment in attachments)
        {
            MessageAttachment? newAttachment = await TryCreateNewAttachmentAsync(message.Id, attachment, cancellationToken);

            if (newAttachment is not null)
            {
                message.AddAttachment(newAttachment);
            }
        }
    }
    
    private async Task<MessageAttachment?> TryCreateNewAttachmentAsync(Guid messageId, IFormFile attachment, CancellationToken cancellationToken = default)
    {
        MessageAttachment newAttachment = new(
            messageId,
            attachment.FileName,
            attachment.ContentType,
            attachment.Length);

        Result<NoValue> storageResult = await _storageService.UploadAsync(
            _storageOptions.ContainerName,
            newAttachment.StorageKey, 
            attachment.OpenReadStream(),
            attachment.ContentType,
            cancellationToken);

        if (storageResult.IsFailure)
        {
            _logger.LogError("Failed to upload attachment for message {MessageId}", messageId);
            return null;
        }

        return newAttachment;
    }
}