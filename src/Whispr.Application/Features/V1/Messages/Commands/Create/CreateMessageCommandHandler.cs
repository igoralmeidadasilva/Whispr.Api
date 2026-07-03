using Whispr.Domain.Features.Entities.Messages;
using Whispr.Domain.Features.Entities.Users;

namespace Whispr.Application.Features.V1.Messages.Commands.Create;

internal sealed class CreateMessageCommandHandler : ICommandHandler<CreateMessageCommand, Unit>
{
    private readonly IMessagePersistenceRepository _messagePersistenceRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMessageCommandHandler(
        IMessagePersistenceRepository messagePersistenceRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _messagePersistenceRepository = messagePersistenceRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        bool userExists = await _userReadOnlyRepository.ExistsAsync(request.UserId, cancellationToken);

        if (!userExists)
        {
            return Result<Unit>.Failure(CreateMessageCommandErrors.UserNotFound);
        }

        Message newMessage = new(request.UserId, request.Content);
        _messagePersistenceRepository.Insert(newMessage);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}