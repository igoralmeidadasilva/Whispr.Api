using Whispr.Application.Features.V1.Messages.Events.MessageCreated;
using Whispr.Domain.Features.Entities.Messages;
using Whispr.Domain.Features.Entities.Users;

namespace Whispr.Application.Features.V1.Messages.Commands.Create;

internal sealed class CreateMessageCommandHandler : ICommandHandler<CreateMessageCommand, Unit>
{
    private readonly IMessagePersistenceRepository _messagePersistenceRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public CreateMessageCommandHandler(
        IMessagePersistenceRepository messagePersistenceRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork,
        IPublisher publisher)
    {
        _messagePersistenceRepository = messagePersistenceRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<Result<Unit>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userReadOnlyRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result<Unit>.Failure(CreateMessageCommandErrors.UserNotFound);
        }

        Message newMessage = new(request.UserId, request.Content);
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
}