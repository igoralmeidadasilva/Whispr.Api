using Whispr.Domain.Features.Entities.Messages;

namespace Whispr.Application.Features.V1.Messages.Commands.Delete;

internal sealed class DeleteMessageCommandHandler : ICommandHandler<DeleteMessageCommand, Unit>
{
    private readonly IMessagePersistenceRepository _messagePersistenceRepository;
    private readonly IMessageReadOnlyRepository _messageReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMessageCommandHandler(
        IMessagePersistenceRepository messagePersistenceRepository,
        IMessageReadOnlyRepository messageReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _messagePersistenceRepository = messagePersistenceRepository;
        _messageReadOnlyRepository = messageReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        Message? message = await _messageReadOnlyRepository.GetByIdAsync(request.MessageId, cancellationToken);

        if (message is null)
        {
            return Result<Unit>.Failure(DeleteMessageCommandErrors.MessageNotFound);
        }

        _messagePersistenceRepository.Delete(message);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}