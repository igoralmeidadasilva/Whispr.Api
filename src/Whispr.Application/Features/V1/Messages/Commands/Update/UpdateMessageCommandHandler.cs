using Whispr.Domain.Features.Entities.Messages;

namespace Whispr.Application.Features.V1.Messages.Commands.Update;

internal sealed class UpdateMessageCommandHandler : ICommandHandler<UpdateMessageCommand, Unit>
{
    private readonly IMessagePersistenceRepository _messagePersistenceRepository;
    private readonly IMessageReadOnlyRepository _messageReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMessageCommandHandler(
        IMessagePersistenceRepository messagePersistenceRepository,
        IMessageReadOnlyRepository messageReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _messagePersistenceRepository = messagePersistenceRepository;
        _messageReadOnlyRepository = messageReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        Message? message = await _messageReadOnlyRepository.GetByIdAsync(request.MessageId, cancellationToken);

        if (message is null)
        {
            return Result<Unit>.Failure(UpdateMessageCommandErrors.MessageNotFound);
        }

        message.Update(request.Content);
        _messagePersistenceRepository.Update(message);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}